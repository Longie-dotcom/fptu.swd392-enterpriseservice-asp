using Application.DTO;

namespace Application.Interface.IService
{
    public interface IEnterpriseService
    {
        Task<IEnumerable<EnterpriseDTO>> GetEnterprises(
            QueryEnterpriseDTO dto, 
            Guid callerId);

        Task<EnterpriseDetailDTO> GetEnterpriseDetail(
            Guid enterpriseId, 
            Guid callerId, 
            string callerRole);

        Task<EnterpriseDetailDTO> GetMyEnterpriseProfile(
            Guid callerId,
            string callerRole);

        Task<IEnumerable<WasteTypeDTO>> GetWasteTypes();

        Task CreateEnterprise(
            CreateEnterpriseDTO dto);

        Task CreateRewardPolicy(
            CreateRewardPolicyDTO dto,
            Guid callerId);

        Task CreateCapacity(
            CreateCapacityDTO dto,
            Guid callerId);

        Task CreateMember(
            CreateMemberDTO dto, 
            Guid callerId);

        Task CreateCollectionAssignment(
            CreateCollectionAssignmentDTO dto, 
            Guid callerId);

        Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);
    }
}