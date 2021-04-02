using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyMessages.Logics.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadPictureAsync(IFormFile picture);
        Task<string> UploadFileAsync(IFormFile file);
        void RemoveFile(string relativePath);
    }
}
