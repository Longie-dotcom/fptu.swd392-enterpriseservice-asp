namespace Domain.Entity
{
    public class RewardPolicy
    {
        #region Attributes
        private readonly List<BonusRule> bonusRules = new List<BonusRule>();
        private readonly List<PenaltyRule> penaltyRules = new List<PenaltyRule>();
        #endregion

        #region Properties
        public Guid RewardPolicyID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int BasePoint { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime ExpiredDate { get; private set; }

        public IReadOnlyCollection<BonusRule> BonusRules
        {
            get { return bonusRules.AsReadOnly(); }
        }
        public IReadOnlyCollection<PenaltyRule> PenaltyRules
        {
            get { return penaltyRules.AsReadOnly(); }
        }

        public Guid EnterpriseID { get; private set; }
        #endregion

        protected RewardPolicy() { }

        public RewardPolicy(
            Guid rewardPolicyId, 
            Guid enterpriseId, 
            string name,
            string description, 
            int basePoint)
        {
            RewardPolicyID = rewardPolicyId;
            EnterpriseID = enterpriseId;
            Name = name;
            Description = description;
            BasePoint = basePoint;
            EffectiveDate = DateTime.UtcNow;
        }

        #region Methods
        public void Deactivate()
        {
            ExpiredDate = DateTime.UtcNow;
        }
        #endregion
    }
}
