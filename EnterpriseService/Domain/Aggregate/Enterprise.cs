using Domain.DomainException;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Aggregate
{
    public class Enterprise
    {
        #region Attributes
        private readonly List<RewardPolicy> rewardPolicies = new List<RewardPolicy>();
        private readonly List<Capacity> capacities = new List<Capacity>();
        private readonly List<Member> members = new List<Member>();
        #endregion

        #region Properties
        public Guid EnterpriseID { get; private set; }
        public Guid UserID { get; private set; }
        public string Name { get; private set; }
        public string TIN { get; private set; }
        public string AvatarName { get; private set; }
        public string Address { get; private set; }
        public string ContactInfo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<RewardPolicy> RewardPolicies
        {
            get { return rewardPolicies.AsReadOnly(); }
        }
        public IReadOnlyCollection<Capacity> Capacities
        {
            get { return capacities.AsReadOnly(); }
        }
        public IReadOnlyCollection<Member> Members
        {
            get { return members.AsReadOnly(); }
        }
        #endregion

        protected Enterprise() { }

        public Enterprise(
            Guid enterpriseId,
            Guid userID,
            string name,
            string tin,
            string avatarName,
            string address,
            string contactInfo,
            bool isActive = true)
        {
            ValidateIdentity(
                enterpriseId,
                name,
                tin,
                address,
                contactInfo);

            EnterpriseID = enterpriseId;
            UserID = userID;
            Name = name;
            TIN = tin;
            AvatarName = avatarName;
            Address = address;
            ContactInfo = contactInfo;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
            IsActive = isActive;
        }

        #region Methods
        public void Deactive()
        {
            IsActive = false;
        }

        public Member CreateMember(
            Guid memberId,
            Guid userId)
        {
            var member = new Member(
                memberId,
                EnterpriseID,
                userId);

            members.Add(member);
            return member;
        }

        public CollectionAssignment AddCollectionAssignment(
            Guid collectionReportId,
            string regionCode,
            Guid assigneeId,
            string note,
            PriorityLevel priorityLevel)
        {
            var capacity = capacities
                .FirstOrDefault(c => c.RegionCode == regionCode && c.ClosedAt == default);
            if (capacity == null)
            {
                throw new EnterpriseAggregateException(
                    $"No active capacity found for region code: {regionCode}");
            }

            var collectionAssignment = capacity.AddCollectionAssignment(
                collectionReportId,
                assigneeId,
                note,
                priorityLevel);

            return collectionAssignment;
        }

        public (string note, int point) CalculateRewardPoint(bool isCorrected, List<Guid> bonusRuleId, List<Guid> penaltyRuleId)
        {
            string note = string.Empty;
            int point = 0;

            var policy = rewardPolicies
                .FirstOrDefault(rp => rp.ExpiredDate == default);
            if (policy == null)
            {
                throw new EnterpriseAggregateException(
                    "No active reward policy found for this enterprise.");
            }
            if (!isCorrected)
            {
                foreach (var penaltyId in penaltyRuleId)
                {
                    var penaltyRule = policy.PenaltyRules
                        .FirstOrDefault(pr => pr.PenaltyRuleID == penaltyId);
                    if (penaltyRule != null)
                    {
                        note += $"Penalty Applied: {penaltyRule.Description}. ";
                        point += penaltyRule.PenaltyPoint;
                    }
                }
            }
            else
            {
                point += policy.BasePoint;
                note += "Base Point Awarded. ";
                foreach (var bonusId in bonusRuleId)
                {
                    var bonusRule = policy.BonusRules
                        .FirstOrDefault(br => br.PenaltyRuleID == bonusId);
                    if (bonusRule != null)
                    {
                        note += $"Bonus Applied: {bonusRule.Description}. ";
                        point += bonusRule.BonusPoint;
                    }
                }
                foreach (var penaltyId in penaltyRuleId)
                {
                    var penaltyRule = policy.PenaltyRules
                        .FirstOrDefault(pr => pr.PenaltyRuleID == penaltyId);
                    if (penaltyRule != null)
                    {
                        note += $"Penalty Removed: {penaltyRule.Description}. ";
                        point += penaltyRule.PenaltyPoint;
                    }
                }
            }
            return (note, point);
        }
        #endregion

        #region Private Validator
        private void ValidateIdentity(
            Guid enterpriseId,
            string name,
            string tin,
            string address,
            string contactInfo)
        {
            if (enterpriseId == Guid.Empty
                || string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(tin)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(contactInfo))
                throw new EnterpriseAggregateException(
                    "Enterprise must have name, TIN, address and contact information/phone number)");
        }
        #endregion
    }
}
