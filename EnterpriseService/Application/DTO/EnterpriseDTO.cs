using Domain.Enum;

namespace Application.DTO
{
    // Enterprise
    public class EnterpriseDTO
    {
        public Guid EnterpriseID { get; set; }
        public Guid UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TIN { get; set; } = string.Empty;
        public string AvatarName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class EnterpriseDetailDTO
    {
        public Guid EnterpriseID { get; set; }
        public Guid UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TIN { get; set; } = string.Empty;
        public string AvatarName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<RewardPolicyDTO> RewardPolicies { get; set; } = new List<RewardPolicyDTO>();
        public List<CapacityDTO> Capacities { get; set; } = new List<CapacityDTO>();
        public List<MemberDTO> Members { get; set; } = new List<MemberDTO>();
    }

    public class QueryEnterpriseDTO
    {
        public string Name { get; set; } = string.Empty;
        public string TIN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateEnterpriseDTO
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string TIN { get; set; } = string.Empty;
        public string AvatarName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }

    // Reward Policy
    public class RewardPolicyDTO
    {
        public Guid RewardPolicyID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BasePoint { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public List<BonusRuleDTO> BonusRules { get; set; } = new List<BonusRuleDTO>();
        public List<PenaltyRuleDTO> PenaltyRules { get; set; } = new List<PenaltyRuleDTO>();
        public Guid EnterpriseID { get; set; }
    }

    public class CreateRewardPolicyDTO
    {

    }

    // Bonus Rule
    public class BonusRuleDTO
    {
        public Guid BonusRuleID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BonusPoint { get; set; }
        public bool IsActive { get; set; }
        public Guid RewardPolicyID { get; set; }
    }

    public class CreateBonusRuleDTO
    {

    }

    // Penalty Rule
    public class PenaltyRuleDTO
    {
        public Guid PenaltyRuleID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PenaltyPoint { get; set; }
        public bool IsActive { get; set; }
        public Guid RewardPolicyID { get; set; }
    }

    public class CreatePenaltyRuleDTO
    {

    }

    // Capacity
    public class CapacityDTO
    {
        public Guid CapacityID { get; set; }
        public double MaxDailyCapacity { get; set; }
        public string RegionCode { get; set; } = string.Empty;
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public double CurrentLoad { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ClosedAt { get; set; }
        public List<CollectionAssignmentDTO> CollectionAssignments { get; set; } = new List<CollectionAssignmentDTO>();
        public string WasteType { get; set; } = string.Empty;
        public Guid EnterpriseID { get; set; }
    }

    public class CreateCapacityDTO
    {

    }

    // Member
    public class MemberDTO
    {
        public Guid MemberID { get; set; }
        public Guid UserID { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime UnassignedAt { get; set; }
        public Guid EnterpriseID { get; set; }
    }

    public class CreateMemberDTO
    {
        public string ContactInfo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    // Collection Assignment
    public class CollectionAssignmentDTO
    {
        public Guid CollectionAssignmentID { get; set; }
        public Guid CollectionReportID { get; set; }
        public Guid AssigneeID { get; set; }
        public string Note { get; set; } = string.Empty;
        public PriorityLevel PriorityLevel { get; set; }
        public DateTime AcceptedAt { get; set; }
        public CollectionReportStatus Status { get; set; }
        public Guid CapacityID { get; set; }
    }

    public class AcceptReportDTO
    {
        public Guid CollectionReportID { get; set; }
        public Guid CapacityID { get; set; }
        public Guid AssigneeID { get; set; } // UserID of Collector
        public PriorityLevel Priority { get; set; }
        public string WasteType { get; set; } = string.Empty;
        public bool IsCorrected { get; set; }
        public string Note { get; set; } = string.Empty;
        public List<Guid> BonusRuleIDs { get; set; } = new List<Guid>();
        public List<Guid> PenaltyRuleIDs { get; set; } = new List<Guid>();
    }
}
