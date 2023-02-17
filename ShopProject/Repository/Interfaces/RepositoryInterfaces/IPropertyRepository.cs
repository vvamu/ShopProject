using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface IPropertyRepository : IBaseRepository<Property>
    {
        Task<IEnumerable<Property>> GetByCollectionAsync(long id);
        Task CreateRangeAsync(IEnumerable<Property> properties);
    }
}