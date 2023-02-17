//using SQLitePCL;
using ShopProject.Data;
using System.Reflection;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace ShopProject.Helper;

interface IRepository<T> : IDisposable
        where T : class
{
    IEnumerable<T> GetList(); // получение всех объектов
    T Get(int id); // получение одного объекта по id
    void Create(T item); // создание объекта
    void Update(T item); // обновление объекта
    void Delete(int id); // удаление объекта по id
    void Save();  // сохранение изменений
}
public class RepositoryServise <T> where T : class
{
    private ApplicationDbContext _context;

    RepositoryServise(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(T item)
    {
        if(item is User user)
        {
            _context.Users.Add(user);
        }

        _context.SaveChanges();
    }
    public void Update(User item)
    {
        throw new NotImplementedException();
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User Get(int id)
    {
        throw new NotImplementedException();
    }


}
