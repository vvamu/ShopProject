using Microsoft.EntityFrameworkCore;
using ShopProject.Data;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;

namespace ShopProject.Repository
{
    public class LikeRepository : ILikeRepository
    {
        ApplicationDbContext _db;
        public LikeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        #region Base

        
        public async Task CreateAsync(Like entity)
        {
            await _db.Likes.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Like entity)
        {
            _db.Likes.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Like>> GetAllAsync()
        {
            return _db.Likes.AsEnumerable<Data.Like>();
        }


        public async Task<Like?> GetAsync(long id)
        {
            return await _db.Likes.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Like Like)
        {
            _db.Likes.Update(Like);
            await _db.SaveChangesAsync();
        }

        #endregion
        public async Task<Like?> GetByUserAndItemAsync(string userId, long itemId)
        {
            return await _db.Likes
                .Where(x => x.UserId == userId)
                .Where(x=>x.ItemId == itemId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Like>?>? GetAllByUserAsync(string userId)
        {
            return _db.Likes.Where(x => x.UserId == userId) ;
        }

        public async Task<IEnumerable<Like>?>? GetAllByItemAsync(long itemId)
        {
            return _db.Likes.Where(x => x.ItemId == itemId);
        }

        public async Task DeleteByUserAsync(string userId)
        {
            var likes = _db.Likes.Where(x => x.UserId == userId);
            _db.Likes.RemoveRange(likes);

            await _db.SaveChangesAsync();
        }
    }
}
