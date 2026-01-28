using Domain.Aggregate;
using Domain.IRepository;

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
