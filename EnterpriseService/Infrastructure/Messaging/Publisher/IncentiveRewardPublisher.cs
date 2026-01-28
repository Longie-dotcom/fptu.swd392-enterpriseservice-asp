using Application.Helper;
using Application.Interface.IPublisher;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Publisher
{
    public class IncentiveRewardPublisher : IIcentiveRewardPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public IncentiveRewardPublisher(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task RewardIncentive(IncentiveRewardDTO dto)
        {
            ServiceLogger.Logging(
                Level.Infrastructure, $"Publishing reward point: {dto.Point} for collection report with ID: {dto.CollectionReportID}");
            await _publishEndpoint.Publish(dto);
        }
    }
}