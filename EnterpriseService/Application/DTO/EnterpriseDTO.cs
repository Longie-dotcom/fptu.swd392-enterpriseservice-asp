using Domain.Enum;

namespace Application.DTO
{
    // Enterprise
    public class EnterpriseDTO
    {
        public Guid EnterpriseID { get; set; }
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

    }

    public class CreateRewardPolicyDTO
    {

    }

    // Capacity
    public class CapacityDTO
    {

    }

    public class CreateCapacityDTO
    {

    }

    // Member
    public class MemberDTO
    {

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

    // Accept Report DTO
    public class AcceptReportDTO
    {
        public List<Guid> BonusRuleId { get; set; } = new List<Guid>();
        public List<Guid> PenaltyRuleId { get; set; } = new List<Guid>();
        public Guid CollectionReportID { get; set; }
        public Guid CollectorID { get; set; }
        public PriorityLevel Priority { get; set; }
        public string WasteType { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
        public bool IsCorrected { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
