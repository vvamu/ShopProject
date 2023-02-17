using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllByCollectionAsync(long id);
        Task<IEnumerable<Item>> GetPrivateByCollectionAsync(long id);
        Task<IEnumerable<Item>> GetPublicByCollectionAsync(long id);

        Task DeleteRangeAsync(IEnumerable<Item> entities);
    }
}
