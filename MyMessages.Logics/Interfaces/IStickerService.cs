using MyMessages.Logics.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMessages.Logics.Interfaces
{
    public interface IStickerService
    {
        Task<List<StickerDto>> GetAllAsync();
    }
}
