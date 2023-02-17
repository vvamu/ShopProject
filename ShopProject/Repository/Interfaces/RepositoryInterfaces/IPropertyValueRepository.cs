using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface IPropertyValueRepository : IBaseRepository<PropertyValue>
    {
        Task<IEnumerable<PropertyValue>> GetByCollectionAsync(long id);

        Task<IEnumerable<PropertyValue>> GetByItemAsync(long id);

        Task CreateRangeAsync(IEnumerable<PropertyValue> properties);


    }
}