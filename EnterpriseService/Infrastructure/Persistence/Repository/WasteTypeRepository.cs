using Domain.Aggregate;
using Domain.Entity;
using Domain.IRepository;
using Infrastructure.InfrastructureException;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class WasteTypeRepository :
        GenericRepository<WasteType>,
        IWasteTypeRepository
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion
        public WasteTypeRepository(EnterpriseDBContext context) : base(context) { }

        #region Methods
        #endregion
    }
}
