using Application.Helper;
using Application.Interface.IPublisher;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Publisher
{
    public class CollectorProfilePublisher : ICollectorProfilePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CollectorProfilePublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task CreateCollectorProfile(CollectorProfileDTO dto)
        {
            ServiceLogger.Logging(
                Level.Infrastructure, $"Publishing create collector profile for user {dto.UserID}");
            await _publishEndpoint.Publish(dto);
        }

    }
}
