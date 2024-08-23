using Models;

namespace Interfaces.IRepository
{
    public interface IBaseRepository<T>
    {
        public IEnumerable<T> GetAll();
        public T? GetById(long id);
        public T? Save(T user);
        public void DeleteById(long id);
    }
}
