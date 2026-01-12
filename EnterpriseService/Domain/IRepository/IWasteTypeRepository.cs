using Domain.Aggregate;
namespace Domain.IRepository
{
    public interface IWasteTypeRepository :
        IGenericRepository<WasteType>,
        IRepositoryBase
    {

    }
}
