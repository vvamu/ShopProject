using Microsoft.EntityFrameworkCore;
using ShopProject.Data;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;
using Collection = ShopProject.Data.Collection;

namespace ShopProject.Repository
{
    public class CollectionRepository : ICollectionRepository
    {
        ApplicationDbContext _db;
        public CollectionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        #region Base

       
        public async Task CreateAsync(Data.Collection entity)
        {
            await _db.Collections.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Data.Collection entity)
        {
            _db.Collections.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Data.Collection>> GetAllAsync()
        {
            return _db.Collections.AsEnumerable<Data.Collection>();
        }

        public async Task<Data.Collection?> GetAsync(long id)
        {
            return await _db.Collections.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(Data.Collection collection)
        {
            _db.Collections.Update(collection);
            await _db.SaveChangesAsync();
        }

        #endregion

        public async Task<IEnumerable<Collection>?>? GetAllByUserAsync(string name)
        {
            
            return _db.Collections.Where(x => x.UserId == name).AsEnumerable();
        }

        public async Task<IEnumerable<Collection>?>? GetPrivateByUserAsync(string name)
        {

            return _db.Collections.Where(x => x.UserId == name)
                .Where(x => x.IsPrivate == true)
                .AsEnumerable();
        }

        public async Task<IEnumerable<Collection>?>? GetPublicByUserAsync(string name)
        {

            return _db.Collections.Where(x => x.UserId == name)
                .Where(x => x.IsPrivate == false)
                .AsEnumerable();
        }
        public async Task DeleteByUserAsync(string name)
        {


            foreach (var ch in _db.Collections.Where(x => x.UserId == name))
                _db.Collections.Remove(ch);

            await _db.SaveChangesAsync();

                
                
        }





    }
}
