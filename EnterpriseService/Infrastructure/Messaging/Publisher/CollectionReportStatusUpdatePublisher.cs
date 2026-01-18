using Application.Helper;
using Application.Interface.IPublisher;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Publisher
{
    public class CollectionReportStatusUpdatePublisher : ICollectionReportStatusUpdatePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public CollectionReportStatusUpdatePublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task UpdateCollectionReportStatus(CollectionReportStatusUpdateDTO dto)
        {
            ServiceLogger.Logging(
                            Level.Infrastructure, $"Updated status: {dto.Status}");
            await _publishEndpoint.Publish(dto);
        }
    }
}