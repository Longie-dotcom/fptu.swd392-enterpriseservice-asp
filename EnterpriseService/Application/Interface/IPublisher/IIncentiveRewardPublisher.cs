using SWD392.MessageBroker;

namespace Application.Interface.IPublisher
{
    public interface IIcentiveRewardPublisher
    {
        Task RewardIncentive(IncentiveRewardDTO dto);
    }
}
