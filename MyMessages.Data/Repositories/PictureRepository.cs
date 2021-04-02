using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;
using MyMessages.Data.Infrastructure;
using MyMessages.Data.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMessages.Data.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly AppDbContext db;

        public PictureRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<Picture> FindByIdAsync(int id, params Expression<Func<Picture, object>>[] includes)
        {
            return db.Pictures
                .IncludeMultiple(includes)
                .FirstOrDefaultAsync(picture => picture.Id == id);
        }

        public void Remove(Picture picture)
        {
            db.Pictures.Remove(picture);
        }
    }
}
