using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ShopProject.Data;
using ShopProject.Repository.Interfaces;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;

namespace ShopProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        private ICollectionRepository _collectionRepository;
        private IItemRepository _itemRepository;
        private IPropertyRepository _propertyRepository;
        private IPropertyValueRepository _propertyValueRepository;
        private ILikeRepository _likeRepository;
        private IUserRepository _userRepository;

        public ICollectionRepository CollectionRepository => _collectionRepository ?? new CollectionRepository(_db);
        public IItemRepository ItemRepository => _itemRepository ?? new ItemRepository(_db); 

        public IPropertyRepository PropertyRepository => _propertyRepository ?? new PropertyRepository(_db);

        public IPropertyValueRepository PropertyValueRepository => _propertyValueRepository?? new PropertyValueRepository(_db);
        public ILikeRepository LikeRepository => _likeRepository ?? new LikeRepository(_db);
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
