using MyMessages.Data.Entities;
using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByIdAsync(int id);
        Task<User> FindByName(string name);
        Task<User> FindByNameAndPasswordHashAsync(string name, string passwordHash);
        void Add(User user);
    }
}
