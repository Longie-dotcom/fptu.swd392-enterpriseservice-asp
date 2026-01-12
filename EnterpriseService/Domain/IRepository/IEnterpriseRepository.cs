using Domain.Aggregate;
namespace Domain.IRepository
{
    public interface IEnterpriseRepository : 
        IGenericRepository<Enterprise>, 
        IRepositoryBase
    {

    }
}
