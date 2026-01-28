using Application.ApplicationException;
using Application.DTO;
using Application.Enum;
using Application.Interface.IGrpcClient;
using Application.Interface.IPublisher;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.IRepository;
using Google.Protobuf.WellKnownTypes;
using IAMServer.gRPC;
using SWD392.MessageBroker;

namespace Application.Service
{
    public class EnterpriseService : IEnterpriseService
    {
        #region Attributes
        private readonly IIAMClient iAMClient;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICollectorProfilePublisher collectorProfilePublisher;
        private readonly ICollectionReportStatusUpdatePublisher collectionReportStatusUpdatePublisher;
        private readonly ICollectionTaskCreatePublisher collectionTaskCreatePublisher;
        private readonly IIcentiveRewardPublisher icentiveRewardPublisher;
        #endregion

        #region Properties
        #endregion

        public EnterpriseService(
            IIAMClient iAMClient,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICollectorProfilePublisher collectorProfilePublisher,
            ICollectionReportStatusUpdatePublisher collectionReportStatusUpdatePublisher,
            IIcentiveRewardPublisher icentiveRewardPublisher,
            ICollectionTaskCreatePublisher collectionTaskCreatePublisher)
        {
            this.iAMClient = iAMClient;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.collectorProfilePublisher = collectorProfilePublisher;
            this.collectionReportStatusUpdatePublisher = collectionReportStatusUpdatePublisher;
            this.icentiveRewardPublisher = icentiveRewardPublisher;
            this.collectionTaskCreatePublisher = collectionTaskCreatePublisher;
        }

        #region Methods
        public async Task<IEnumerable<EnterpriseDTO>> GetEnterprises(
            QueryEnterpriseDTO dto,
            Guid callerId)
        {
            // Validate enterprise list existence
            var list = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .QueryEnterprises(
                dto.Name,
                dto.TIN,
                dto.Address,
                dto.ContactInfo,
                dto.IsActive);

            if (list == null || !list.Any())
                throw new EnterpriseNotFound(
                    "Enterprises list is not found or empty");

            return mapper.Map<IEnumerable<EnterpriseDTO>>(list);
        }

        public async Task<EnterpriseDetailDTO> GetEnterpriseDetail(
            Guid enterpriseId,
            Guid callerId, 
            string callerRole)
        {
            // Validate enterprise list existence
            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetEnterpriseDetailByIdAsync(enterpriseId);

            if (enterprise == null)
                throw new EnterpriseNotFound(
                    $"Enterprise with ID: {enterpriseId} is not found");

            // Authorize access
            if (enterprise.UserID != callerId && callerRole != RoleKey.ADMIN)
                throw new EnterpriseNotFound(
                    $"User can not access this enterprise profile");

            return mapper.Map<EnterpriseDetailDTO>(enterprise);
        }

        public async Task CreateEnterprise(
            CreateEnterpriseDTO dto)
        {
            // Create user from IAM Service
            var response = await iAMClient.CreateUser(new CreateUserRequest()
            {
                Dob = Timestamp.FromDateTime(dto.Dob.ToUniversalTime()),
                Email = dto.Email,
                FullName = dto.FullName,
                Gender = dto.Gender,
                IsActive = true,
                Password = dto.Password,
                Role = RoleKey.ENTERPRISE,
            });

            // Apply domain
            var enterprise = new Enterprise(
                Guid.NewGuid(),
                Guid.Parse(response.UserId),
                dto.Name,
                dto.TIN,
                dto.AvatarName,
                dto.Address,
                dto.ContactInfo,
                true);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .Add(enterprise);
            await unitOfWork.CommitAsync(response.UserId);
        }

        public async Task CreateMember(
            CreateMemberDTO dto, 
            Guid callerId)
        {
            // Create collector account from IAM service
            var user = await iAMClient.CreateUser(new CreateUserRequest()
            {
                Dob = Timestamp.FromDateTime(dto.Dob.ToUniversalTime()),
                Email = dto.Email,
                FullName = dto.FullName,
                Gender = dto.Gender,
                Password = dto.Password,
                Role = RoleKey.COLLECTOR,
                IsActive = true
            });

            // Validate enterprise existence
            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetEnterpriseByUserIdAsync(callerId);

            if (enterprise == null)
                throw new EnterpriseNotFound(
                    "Enterprise not found");

            // Apply domain
            var member = enterprise.CreateMember(
                Guid.NewGuid(), 
                Guid.Parse(user.UserId));

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .AddMember(member);
            await unitOfWork.CommitAsync(user.UserId);

            // Publish to Collection Service
            await collectorProfilePublisher.CreateCollectorProfile(new CollectorProfileDTO()
            {
                ContactInfo = dto.ContactInfo,
                UserID = Guid.Parse(user.UserId),
                IsActive = true
            });
        }

        public async Task AcceptReport(
            AcceptReportDTO dto,
            Guid callerId)
        {
            // Validate enterprise existence
            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetEnterpriseByUserIdAsync(callerId);

            if (enterprise == null)
                throw new EnterpriseNotFound(
                    "Enterprise not found");

            // Apply domain
            var collectionAssignment = enterprise.AddCollectionAssignment(
                dto.CollectionReportID,
                dto.CapacityID,
                dto.AssigneeID,
                dto.Note,
                dto.Priority,
                dto.WasteType);

            var (note, point) = enterprise.CalculateRewardPoint(
                dto.IsCorrected,
                dto.BonusRuleIDs,
                dto.PenaltyRuleIDs);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .AddCollectionAssignment(collectionAssignment);
            await unitOfWork.CommitAsync(callerId.ToString());

            // Publish to Citizen Service
            await icentiveRewardPublisher.RewardIncentive(new IncentiveRewardDTO()
            {
                CollectionReportID = dto.CollectionReportID,
                Reason = note,
                Point = point
            });

            // Publish to Citizen Service
            await collectionReportStatusUpdatePublisher.UpdateCollectionReportStatus(new CollectionReportStatusUpdateDTO()
            {
                CollectionReportID = dto.CollectionReportID,
                Status = collectionAssignment.Status.ToString()
            });

            // Publish to Collection Service
            await collectionTaskCreatePublisher.CreateCollectionTask(new CollectionTaskCreateDTO()
            {
                CollectionReportID = collectionAssignment.CollectionReportID,
                CollectorProfileID = collectionAssignment.AssigneeID
            });
        }

        public async Task UserSyncDeleting(UserDeleteDTO dto)
        {
            // Validate enterprise existence
            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetEnterpriseByUserIdAsync(dto.UserID);

            if (enterprise == null)
                throw new EnterpriseNotFound(
                    $"The enterprise with user ID: {dto.UserID} is not found");

            // Apply domain
            enterprise.Deactive();

            // Apply persistence;
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .Update(enterprise.EnterpriseID, enterprise);
            await unitOfWork.CommitAsync();
        }
        #endregion
    }
}
