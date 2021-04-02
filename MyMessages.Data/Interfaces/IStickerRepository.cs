using MyMessages.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IStickerRepository
    {
        Task<Sticker> FindByIdAsync(int id);
        Task<List<Sticker>> FindAllAsync();
    }
}
