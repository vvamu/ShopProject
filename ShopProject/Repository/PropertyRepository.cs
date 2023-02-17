using Microsoft.EntityFrameworkCore;
using ShopProject.Data;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;

namespace ShopProject.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        ApplicationDbContext _db;
        public PropertyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Base

        
        public async Task CreateAsync(Property entity)
        {
            await _db.Properties.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Property entity)
        {
            _db.Properties.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return  _db.Properties.AsEnumerable<Data.Property>();
        }


        public async Task<Property?> GetAsync(long id)
        {
            return await _db.Properties.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task UpdateAsync(Property Property)
        {
            _db.Properties.Update(Property);
            await _db.SaveChangesAsync();
        }

        #endregion
        public async Task<IEnumerable<Property>> GetByCollectionAsync(long collId)
        {
            return await _db.Properties
                .Where(x=>x.CollectionId == collId)
                .ToListAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Property> properties)
        {
            await _db.AddRangeAsync(properties);
            await _db.SaveChangesAsync();


        }




        
    }
}
