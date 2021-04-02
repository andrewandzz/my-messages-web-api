using Microsoft.AspNetCore.StaticFiles;

namespace MyMessages.Logics.Dtos
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }

        public void UpdateContentType()
        {
            new FileExtensionContentTypeProvider().TryGetContentType(Path, out string contentType);
            ContentType = contentType ?? "application/octet-stream";
        }
    }
}
