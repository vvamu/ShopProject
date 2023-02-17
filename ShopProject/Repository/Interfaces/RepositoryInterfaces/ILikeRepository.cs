using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        public Task<Like?> GetByUserAndItemAsync(string userName, long itemId);
        public Task<IEnumerable<Like>?>? GetAllByUserAsync(string userId);
        public Task<IEnumerable<Like>?>? GetAllByItemAsync(long itemId);

        Task DeleteByUserAsync(string userId);

    }
}