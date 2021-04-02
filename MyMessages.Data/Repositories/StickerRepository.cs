using Microsoft.EntityFrameworkCore;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMessages.Data.Repositories
{
    public class StickerRepository : IStickerRepository
    {
        private readonly AppDbContext db;

        public StickerRepository(AppDbContext db)
        {
            this.db = db;
        }

        public Task<List<Sticker>> FindAllAsync()
        {
            return db.Stickers.ToListAsync();
        }

        public Task<Sticker> FindByIdAsync(int id)
        {
            return db.Stickers.FirstOrDefaultAsync(sticker => sticker.Id == id);
        }
    }
}
