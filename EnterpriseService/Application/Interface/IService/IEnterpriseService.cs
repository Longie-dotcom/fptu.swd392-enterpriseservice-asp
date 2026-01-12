using Application.DTO;

namespace Application.Interface.IService
{
    public interface IEnterpriseService
    {
        Task CreateEnterprise(
            CreateEnterpriseDTO dto);

        Task UserSyncDeleting(
            SWD392.MessageBroker.UserDeleteDTO dto);
    }
}