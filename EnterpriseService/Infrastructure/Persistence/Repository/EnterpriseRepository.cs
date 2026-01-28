using Domain.Aggregate;
using Domain.Entity;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class EnterpriseRepository :
        GenericRepository<Enterprise>,
        IEnterpriseRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        public EnterpriseRepository(EnterpriseDBContext context) : base(context) { }

        #region Methods
        public async Task<IEnumerable<Enterprise>> QueryEnterprises(
            string? name,
            string? tin,
            string? address,
            string? contactInfo,
            bool? isActive) // make nullable so it can be optional
        {
            IQueryable<Enterprise> query = context.Enterprises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(e => e.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(tin))
                query = query.Where(e => e.TIN.Contains(tin));

            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(e => e.Address.Contains(address));

            if (!string.IsNullOrWhiteSpace(contactInfo))
                query = query.Where(e => e.ContactInfo.Contains(contactInfo));

            if (isActive.HasValue)
                query = query.Where(e => e.IsActive == isActive.Value);

            // Order by Name
            query = query.OrderBy(e => e.Name);

            return await query.ToListAsync();
        }

        public async Task<Enterprise?> GetEnterpriseDetailByIdAsync(
            Guid enterpriseId)
        {
            return await context.Enterprises
                .Where(e => e.EnterpriseID == enterpriseId)
                .Include(e => e.Members)
                .Include(e => e.Capacities)
                    .ThenInclude(rp => rp.CollectionAssignments)
                .Include(e => e.RewardPolicies)
                    .ThenInclude(rp => rp.BonusRules)
                .Include(e => e.RewardPolicies)
                    .ThenInclude(rp => rp.PenaltyRules)
                .FirstOrDefaultAsync();
        }

        public async Task<Enterprise?> GetEnterpriseByUserIdAsync(
            Guid userId)
        {
            return await context.Enterprises
                .Where(e => e.UserID == userId)
                .Include(e => e.Members)
                .Include(e => e.Capacities)
                    .ThenInclude(rp => rp.CollectionAssignments)
                .Include(e => e.RewardPolicies)
                    .ThenInclude(rp => rp.BonusRules)
                .Include(e => e.RewardPolicies)
                    .ThenInclude(rp => rp.PenaltyRules)
                .FirstOrDefaultAsync();
        }

        public void AddMember(
            Member member)
        {
            context.Members.Add(member);
        }

        public void AddCollectionAssignment(
            CollectionAssignment collectionAssignment)
        {
            context.CollectionAssignments.Add(collectionAssignment);
        }
        #endregion
    }
}
