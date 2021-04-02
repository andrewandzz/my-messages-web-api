using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using System.Threading.Tasks;

namespace MyMessages.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext db;

        public UserRepository(AppDbContext db)
        {
            this.db = db;
        }

        public void Add(User user)
        {
            db.Users.Add(user);
        }

        public Task<User> FindByIdAsync(int id)
        {
            return db.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<User> FindByName(string name)
        {
            return db.Users.FirstOrDefaultAsync(user => user.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
        }

        public Task<User> FindByNameAndPasswordHashAsync(string name, string passwordHash)
        {
            return db.Users.FirstOrDefaultAsync(user =>
                user.Name.Trim().ToLower().Equals(name.Trim().ToLower()) &&
                user.PasswordHash.Equals(passwordHash)
            );
        }
    }
}
