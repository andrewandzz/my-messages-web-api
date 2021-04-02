using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyMessages.Logics.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyMessages.Logics.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public Task<string> UploadPictureAsync(IFormFile picture)
        {
            return UploadFileToDirectory(picture, "pictures");
        }

        public Task<string> UploadFileAsync(IFormFile file)
        {
            return UploadFileToDirectory(file, "files");
        }

        public void RemoveFile(string relativePath)
        {
            var path = Path.Combine(environment.WebRootPath, relativePath);
            File.Delete(path);
        }

        private async Task<string> UploadFileToDirectory(IFormFile file, string directoryName)
        {
            EnsureDirectoryCreated(directoryName);

            var fileExtension = Path.GetExtension(file.FileName);
            var newNameForFile = Guid.NewGuid().ToString() + fileExtension;
            var path = Path.Combine(environment.WebRootPath, directoryName, newNameForFile);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = Path.Combine(directoryName, newNameForFile);

            return relativePath;
        }

        private void EnsureDirectoryCreated(string directoryName)
        {
            var path = Path.Combine(environment.WebRootPath, directoryName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
