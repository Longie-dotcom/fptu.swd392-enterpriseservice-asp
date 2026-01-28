using Domain.DomainException;

namespace Domain.Entity
{
    public class BonusRule
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid BonusRuleID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int BonusPoint { get; private set; }
        public bool IsActive { get; private set; }

        public Guid RewardPolicyID { get; private set; }
        #endregion

        protected BonusRule() { }

        public BonusRule(
            Guid bonusRuleId,
            Guid rewardPolicyId,
            string name,
            string description,
            int bonusPoint,
            bool isActive = true)
        {
            if (bonusPoint <= 0)
                throw new EnterpriseAggregateException(
                    "Bonus point can not be smaller or equal to 0");

            if (string.IsNullOrWhiteSpace(name))
                throw new EnterpriseAggregateException(
                    "Bonus rule name cannot be empty");

            BonusRuleID = bonusRuleId;
            RewardPolicyID = rewardPolicyId;
            Name = name;
            Description = description.Trim();
            BonusPoint = bonusPoint;
            IsActive = isActive;
        }

        #region Methods
        #endregion
    }
}
