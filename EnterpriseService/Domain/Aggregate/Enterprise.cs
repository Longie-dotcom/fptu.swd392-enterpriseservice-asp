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
        public Member CreateMember(
            Guid memberId,
            Guid userId)
        {
            if (!IsActive)
                throw new EnterpriseAggregateException(
                    "Enterprise has been deactive");

            var member = new Member(
                memberId,
                EnterpriseID,
                userId);

            members.Add(member);

            return member;
        }

        public CollectionAssignment AddCollectionAssignment(
            Guid collectionReportId,
            Guid capacityId,
            Guid assigneeId,
            string note,
            PriorityLevel priorityLevel,
            string wasteType)
        {
            if (!IsActive)
                throw new EnterpriseAggregateException(
                    "Enterprise has been deactive");

            // Validate capacity existence
            var capacity = capacities
                .FirstOrDefault(c => c.CapacityID == capacityId && c.ClosedAt == default);
            
            if (capacity == null)
                throw new EnterpriseAggregateException(
                    $"No active capacity found for capacity ID: {capacityId}");

            if (capacity.WasteType != wasteType)
                throw new EnterpriseAggregateException(
                    $"The assigned capacity does not support waste type: {wasteType}");

            var collectionAssignment = capacity.AddCollectionAssignment(
                collectionReportId,
                assigneeId,
                note,
                priorityLevel);

            return collectionAssignment;
        }

        public (string note, int point) CalculateRewardPoint(
            bool isCorrected, 
            List<Guid> bonusRuleIds, 
            List<Guid> penaltyRuleIds)
        {
            if (!IsActive)
                throw new EnterpriseAggregateException(
                    "Enterprise has been deactive");

            string note = string.Empty;
            int point = 0;

            // Validate latest policy existence
            var policy = rewardPolicies
                .FirstOrDefault(rp => rp.ExpiredDate == default);

            if (policy == null)
                throw new EnterpriseAggregateException(
                    "No active reward policy found for this enterprise.");

            // Calculation logic
            if (!isCorrected)
            {
                // Apply penalty only
                foreach (var penaltyId in penaltyRuleIds)
                {
                    var penaltyRule = policy.PenaltyRules
                        .FirstOrDefault(pr => pr.PenaltyRuleID == penaltyId);
                    if (penaltyRule != null)
                    {
                        note += $"Penalty Applied: {penaltyRule.Name}: {penaltyRule.PenaltyPoint}. ";
                        point += penaltyRule.PenaltyPoint;
                    }
                }
            }
            else
            {
                // Apply base point for correct report
                point += policy.BasePoint;
                note += $"Base Point Awarded: {point}. ";

                // Apply bonus point
                note += "Bonus Applied: ";
                foreach (var bonusId in bonusRuleIds)
                {
                    var bonusRule = policy.BonusRules
                        .FirstOrDefault(br => br.BonusRuleID == bonusId);
                    if (bonusRule != null)
                    {
                        note += $"{bonusRule.Name}: {bonusRule.BonusPoint}, ";
                        point += bonusRule.BonusPoint;
                    }
                }

                // Apply penalty point
                note += "Penalty Removed: ";
                foreach (var penaltyId in penaltyRuleIds)
                {
                    var penaltyRule = policy.PenaltyRules
                        .FirstOrDefault(pr => pr.PenaltyRuleID == penaltyId);
                    if (penaltyRule != null)
                    {
                        note += $"{penaltyRule.Name}: {penaltyRule.PenaltyPoint}, ";
                        point += penaltyRule.PenaltyPoint;
                    }
                }
            }
            return (note, point);
        }

        public void Deactive()
        {
            IsActive = false;
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
