using Microsoft.EntityFrameworkCore;
using ShopProject.Data;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;

namespace ShopProject.Repository
{
    public class ItemRepository : IItemRepository
    {
        ApplicationDbContext _db;
        public ItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Base

        
        public async Task CreateAsync(Item entity)
        {
            await _db.Items.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Item entity)
        {
            _db.Items.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return  _db.Items.AsEnumerable<Data.Item>();
        }


        public async Task<Item?> GetAsync(long id)
        {
            return await _db.Items.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task UpdateAsync(Item Item)
        {
            _db.Items.Update(Item);
            await _db.SaveChangesAsync();
        }

        #endregion
        public async Task<IEnumerable<Item>> GetAllByCollectionAsync(long collectionId)
        {
            return await _db.Items
                .Where(x => x.CollectionId == collectionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetPrivateByCollectionAsync(long collectionId)
        {
            return await _db.Items
                .Where(x => x.CollectionId == collectionId)
                .Where(x=>x.IsPrivate == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetPublicByCollectionAsync(long collectionId)
        {
            return await _db.Items
                .Where(x => x.CollectionId == collectionId)
                .Where(x => x.IsPrivate == false)
                .ToListAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<Item> entities)
        {
            _db.Items.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}
