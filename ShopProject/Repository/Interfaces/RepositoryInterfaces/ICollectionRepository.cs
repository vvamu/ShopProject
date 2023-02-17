using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface ICollectionRepository : IBaseRepository<Collection>
    {
        Task<IEnumerable<Collection>?>? GetAllByUserAsync(string userId);

        Task<IEnumerable<Collection>?>? GetPrivateByUserAsync(string userId);
        Task<IEnumerable<Collection>?>? GetPublicByUserAsync(string userId);
        Task DeleteByUserAsync(string userId);
    }
}
