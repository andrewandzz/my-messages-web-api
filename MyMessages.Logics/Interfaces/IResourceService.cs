using MyMessages.Logics.Dtos;
using System.Threading.Tasks;

namespace MyMessages.Logics.Interfaces
{
    public interface IResourceService
    {
        Task<PictureDto> GetPictureByIdAsync(int id, int userId);
        Task<FileDto> GetFileByIdAsync(int id, int userId);
        Task<StickerDto> GetStickerByIdAsync(int id);
    }
}
