using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;
using MyMessages.Data.Infrastructure;
using MyMessages.Data.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext db;

        public FileRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<File> FindByIdAsync(int id, params Expression<Func<File, object>>[] includes)
        {
            return db.Files
                .IncludeMultiple(includes)
                .FirstOrDefaultAsync(file => file.Id == id);
        }

        public void Remove(File file)
        {
            db.Files.Remove(file);
        }
    }
}
