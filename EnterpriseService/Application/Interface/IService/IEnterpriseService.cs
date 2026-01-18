using Application.DTO;

namespace Application.Interface.IService
{
    public interface IEnterpriseService
    {
        Task CreateEnterprise(
            CreateEnterpriseDTO dto);

        Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);

        Task CreateMember(
            CreateMemberDTO dto, Guid callerId);
    }
}