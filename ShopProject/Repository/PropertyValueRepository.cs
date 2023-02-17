using Microsoft.EntityFrameworkCore;
using ShopProject.Data;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;

namespace ShopProject.Repository
{
    public class PropertyValueRepository : IPropertyValueRepository
    {
        ApplicationDbContext _db;
        public PropertyValueRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Base

        
        public async Task CreateAsync(PropertyValue entity)
        {
            await _db.PropertyValues.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(PropertyValue entity)
        {
            _db.PropertyValues.Remove(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<PropertyValue>> GetAllAsync()
        {
            return  _db.PropertyValues.AsEnumerable<Data.PropertyValue>();
        }


        public async Task<PropertyValue?> GetAsync(long id)
        {
            return await _db.PropertyValues.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        public async Task UpdateAsync(PropertyValue PropertyValue)
        {
            _db.PropertyValues.Update(PropertyValue);
            await _db.SaveChangesAsync();
        }

        #endregion
        public async Task<IEnumerable<PropertyValue>> GetByCollectionAsync(long propId)
        {
            return await _db.PropertyValues
                .Where(x=>x.PropertyId == propId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PropertyValue>> GetByItemAsync(long itemId)
        {
            return await _db.PropertyValues
               .Where(x => x.ItemId == itemId)
               .ToListAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<PropertyValue> PropertyValues)
        {
            await _db.AddRangeAsync(PropertyValues);
            await _db.SaveChangesAsync();


        }


    }
}
