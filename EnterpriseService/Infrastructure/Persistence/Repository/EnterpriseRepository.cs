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
        public void AddMember(Member member)
        {
            context.Members.Add(member);
        }

        public async Task<Enterprise?> GetEnterpriseByUserIdAsync(Guid userId)
        {
            return await context.Enterprises
                .Where(e => e.Members.Any(m => m.UserID == userId))
                .FirstOrDefaultAsync();
        }

        public void AddCollectionAssignment(CollectionAssignment collectionAssignment)
        {
            context.CollectionAssignments.Add(collectionAssignment);
        }
        #endregion
    }
}
