using Domain.Aggregate;
using Domain.Entity;
namespace Domain.IRepository
{
    public interface IEnterpriseRepository :
        IGenericRepository<Enterprise>,
        IRepositoryBase
    {
        void AddMember(Member member);

        Task<Enterprise?> GetEnterpriseByUserIdAsync(Guid userId);

        void AddCollectionAssignment(CollectionAssignment collectionAssignment);
    }
}
