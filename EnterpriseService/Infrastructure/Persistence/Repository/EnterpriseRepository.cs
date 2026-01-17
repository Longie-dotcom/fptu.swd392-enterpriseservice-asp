using Domain.Aggregate;
using Domain.Entity;
using Domain.IRepository;

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
        #endregion
    }
}
