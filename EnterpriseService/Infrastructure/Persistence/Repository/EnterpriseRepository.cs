using Domain.Aggregate;
using Domain.Entity;
using Domain.IRepository;
using Infrastructure.InfrastructureException;
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
        #endregion
    }
}
