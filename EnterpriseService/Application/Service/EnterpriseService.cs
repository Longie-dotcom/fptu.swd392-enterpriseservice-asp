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
        #endregion

        #region Properties
        #endregion

        public EnterpriseService(
            IIAMClient iAMClient,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICollectorProfilePublisher collectorProfilePublisher)
        {
            this.iAMClient = iAMClient;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.collectorProfilePublisher = collectorProfilePublisher;
        }

        public async Task CreateMember(CreateMemberDTO dto, Guid callerId)
        {
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

            var enterprise = await unitOfWork
                .GetRepository<IEnterpriseRepository>()
                .GetByIdAsync(callerId);

            if (enterprise == null)
            {
                throw new EnterpriseNotFound("Enterprise not found");
            }

            var member = enterprise.CreateMember(Guid.NewGuid(), Guid.Parse(user.UserId));
            await unitOfWork.BeginTransactionAsync();
            unitOfWork.GetRepository<IEnterpriseRepository>()
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

        #region Methods
        public async Task CreateEnterprise(CreateEnterpriseDTO dto)
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

        public Task UserSyncDeleting(UserDeleteDTO dto)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
