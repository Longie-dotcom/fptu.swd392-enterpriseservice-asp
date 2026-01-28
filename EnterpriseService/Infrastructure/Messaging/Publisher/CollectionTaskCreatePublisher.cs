using Application.Helper;
using Application.Interface.IPublisher;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Publisher
{
    public class CollectionTaskCreatePublisher : ICollectionTaskCreatePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CollectionTaskCreatePublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task CreateCollectionTask(CollectionTaskCreateDTO dto)
        {
            ServiceLogger.Logging(
                Level.Infrastructure, $"Publishing create collection task for collector user {dto.CollectorProfileID}");
            await _publishEndpoint.Publish(dto);
        }
    }
}
