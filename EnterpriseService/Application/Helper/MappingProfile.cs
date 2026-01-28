using Application.DTO;
using AutoMapper;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Waste Type
            // Aggregate
            CreateMap<WasteType, WasteTypeDTO>();
            #endregion

            #region Enterprise
            // Entity
            CreateMap<PenaltyRule, PenaltyRuleDTO>();
            CreateMap<BonusRule, BonusRuleDTO>();
            CreateMap<RewardPolicy, RewardPolicyDTO>()
                .ForMember(dest => dest.PenaltyRules,
                    opt => opt.MapFrom(src => src.PenaltyRules))
                .ForMember(dest => dest.BonusRules,
                    opt => opt.MapFrom(src => src.BonusRules));

            CreateMap<CollectionAssignment, CollectionAssignmentDTO>();
            CreateMap<Capacity, CapacityDTO>()
                .ForMember(dest => dest.CollectionAssignments,
                    opt => opt.MapFrom(src => src.CollectionAssignments));

            CreateMap<Member, MemberDTO>();

            // Aggregate
            CreateMap<Enterprise, EnterpriseDTO>();
            CreateMap<Enterprise, EnterpriseDetailDTO>()
                .ForMember(dest => dest.RewardPolicies,
                    opt => opt.MapFrom(src => src.RewardPolicies))
                .ForMember(dest => dest.Capacities,
                    opt => opt.MapFrom(src => src.Capacities))
                .ForMember(dest => dest.Members,
                    opt => opt.MapFrom(src => src.Members));
            #endregion
        }
    }
}
