namespace Domain.Entity
{
    public class PenaltyRule
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid PenaltyRuleID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int PenaltyPoint { get; private set; }
        public bool IsActive { get; private set; }

        public Guid RewardPolicyID { get; private set; }
        #endregion

        protected PenaltyRule() { }

        public PenaltyRule(
            Guid penaltyRuleId,
            Guid rewardPolicyId,
            string name, 
            string description, 
            int penaltyPoint,
            bool isActive = true)
        {
            PenaltyRuleID = penaltyRuleId;
            RewardPolicyID = rewardPolicyId;
            Name = name;
            Description = description;
            PenaltyPoint = penaltyPoint;
            IsActive = isActive;
        }

        #region Methods
        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
        #endregion
    }
}
