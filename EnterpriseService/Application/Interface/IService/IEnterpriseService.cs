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

        Task CreateEnterprise(
            CreateEnterpriseDTO dto);

        Task CreateMember(
            CreateMemberDTO dto, 
            Guid callerId);

        Task AcceptReport(
            AcceptReportDTO dto, 
            Guid callerId);

        Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);
    }
}