using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ShopProject.Data;

namespace ShopProject.Repository.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository 
    {
        Task LogoutAsync();
        Task<User?> GetUserAsync(ClaimsPrincipal userPrincipal);

        Task<User?> GetUserByIdAsync(string id);
        IEnumerable<User> GetUsers();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<IEnumerable<string>> GetRolesAsync(User user);
        Task<string> GetUserIdAsync(ClaimsPrincipal userPrincipal);

        Task BlockUserAsync(string userId);
        Task UnblockUserAsync(string userId);
        Task DeleteUserAsync(string userId);
        Task AddToAdminAsync(string userId);
        Task DeleteFromAdminAsync(string userId);
        IdentityResult ErrorState { get; protected set; }
    }
}