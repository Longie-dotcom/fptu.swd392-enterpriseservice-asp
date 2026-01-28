using SWD392.MessageBroker;

namespace Application.Interface.IPublisher
{
    public interface ICollectionTaskCreatePublisher
    {
        Task CreateCollectionTask(CollectionTaskCreateDTO dto);
    }
}
