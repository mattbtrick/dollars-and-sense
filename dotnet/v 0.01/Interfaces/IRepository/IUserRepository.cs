using Models;

namespace Interfaces.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public User? GetById(long id, bool populatePermissions);
    }
}
