using Application.Helper;
using Application.Interface.IService;
using MassTransit;
using SWD392.MessageBroker;

namespace Infrastructure.Messaging.Consumer
{
    public class CollectionReportStatusUpdateConsumer : IConsumer<CollectionReportStatusUpdateDTO>
    {
        private readonly IEnterpriseService enterpriseService;

        public CollectionReportStatusUpdateConsumer(
            IEnterpriseService enterpriseService)
        {
            this.enterpriseService = enterpriseService;
        }

        public async Task Consume(ConsumeContext<CollectionReportStatusUpdateDTO> context)
        {
            try
            {
                var message = context.Message;
                ServiceLogger.Logging(
                    Level.Infrastructure, $"Update collection report status: {message.Status}");
                await enterpriseService.UpdateCollectionReportStatus(message);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(
                    Level.Infrastructure, $"Failed when update collection report status: {ex.Message}");
            }
        }
    }
}
