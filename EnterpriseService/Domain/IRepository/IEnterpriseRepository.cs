using Domain.Aggregate;
using Domain.Entity;
namespace Domain.IRepository
{
    public interface IEnterpriseRepository :
        IGenericRepository<Enterprise>,
        IRepositoryBase
    {
        Task<IEnumerable<Enterprise>> QueryEnterprises(
            string? name,
            string? tin,
            string? address,
            string? contactInfo,
            bool? isActive);

        Task<Enterprise?> GetEnterpriseDetailByIdAsync(
            Guid enterpriseId);

        Task<Enterprise?> GetEnterpriseByUserIdAsync(
            Guid userId);

        void AddMember(
            Member member);
        
        void AddCollectionAssignment(
            CollectionAssignment collectionAssignment);
    }
}
