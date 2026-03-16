using Application.ApplicationException;
using Application.DTO;
using Application.Enum;
using Application.Interface.IGrpcClient;
using Application.Interface.IPublisher;
using Application.Interface.IService;
using AutoMapper;
using Domain.Aggregate;
using Domain.Enum;
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
                dto.IsActive,
                dto.PageIndex,
                dto.PageSize);

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
                    $"User can not access this enterprise detail");

            return mapper.Map<EnterpriseDetailDTO>(enterprise);
        }

        public async Task<EnterpriseDetailDTO> GetMyEnterpriseProfile(
            Guid callerId,
            string callerRole)
        {
            // Validate enterprise list existence
            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetEnterpriseByUserIdAsync(callerId);

            if (enterprise == null)
                throw new EnterpriseNotFound(
                    $"Enterprise with user ID: {callerId} is not found");

            var mappedEnterprise = mapper.Map<EnterpriseDetailDTO>(enterprise);

            // Populate collector information with user data
            foreach (var collector in mappedEnterprise.Members)
            {
                var user = await iAMClient.GetUser(new GetUserRequest() 
                { 
                    CreatedBy = callerId.ToString(),
                    Role = callerRole.ToString(),
                    UserId = collector.UserID.ToString(),
                });

                var mappedUser = new UserDTO()
                {
                    Dob = user.Dob.ToDateTime(),
                    Email = user.Email,
                    Gender = user.Gender,
                    FullName = user.FullName,
                    IsActive = user.IsActive,
                };

                collector.UserInformation = mappedUser;
            }

            // Authorize access
            if (enterprise.UserID != callerId && callerRole != RoleKey.ADMIN)
                throw new EnterpriseNotFound(
                    $"User can not access this enterprise detail");

            return mappedEnterprise;
        }

        public async Task<IEnumerable<WasteTypeDTO>> GetWasteTypes()
        {
            // Validate waste type list existence
            var list = await unitOfWork
                .GetRepository<IWasteTypeRepository>()
                .GetAllAsync();

            if (list == null || !list.Any())
                throw new WasteTypeNotFound(
                    $"Waste type list is not found or empty");

            return mapper.Map<IEnumerable<WasteTypeDTO>>(list);
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

        public async Task CreateRewardPolicy(
            CreateRewardPolicyDTO dto, 
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
            var rewardPolicy = enterprise.AddRewardPolicy(
                dto.Name,
                dto.Description,
                dto.BasePoint);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .AddRewardPolicy(rewardPolicy);
            await unitOfWork.CommitAsync(callerId.ToString());
        }

        public async Task CreateCapacity(
            CreateCapacityDTO dto,
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
            var capacity = enterprise.AddCapacity(
                dto.RegionCode,
                dto.UnitOfMeasure,
                dto.WasteType,
                dto.MaxDailyCapacity);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .AddCapacity(capacity);
            await unitOfWork.CommitAsync(callerId.ToString());
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
            var member = enterprise.AddMember(
                Guid.Parse(user.UserId));

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .AddMember(member);
            await unitOfWork.CommitAsync(user.UserId);

            // Publish to Collection Service - create profile
            await collectorProfilePublisher.CreateCollectorProfile(new CollectorProfileDTO()
            {
                ContactInfo = dto.ContactInfo,
                UserID = Guid.Parse(user.UserId),
                IsActive = true
            });
        }

        public async Task CreateCollectionAssignment(
            CreateCollectionAssignmentDTO dto,
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

            Console.WriteLine($"AAAAAAAAAAAAAAAA: {note}, BBBBBBBBBBBBBBBBBBBBBBBBBBBB: {point}");

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
                CollectorUserID = collectionAssignment.AssigneeID
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

        public async Task UpdateCollectionReportStatus(CollectionReportStatusUpdateDTO dto)
        {
            // Validate collection report existence
            var collectionAssigment = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetCollectionAssignmentByReportIdAsync(dto.CollectionReportID);

            if (collectionAssigment == null)
                throw new CollectionAssignmentNotFound(
                    $"The collection report with ID: {dto.CollectionReportID} is not found");

            // Validate status enum
            if (!System.Enum.TryParse<CollectionReportStatus>(dto.Status, true, out var status))
                throw new Exception(
                    $"Invalid collection report status: {dto.Status}");

            // Apply domain
            collectionAssigment.UpdateStatus(status);

            // Apply persistence
            await unitOfWork.BeginTransactionAsync();
            await unitOfWork.CommitAsync();
        }
        #endregion
    }
}
