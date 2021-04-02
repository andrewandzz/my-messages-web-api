using Microsoft.AspNetCore.Http;

namespace MyMessages.Logics.Dtos
{
    public class IncomingMessageDto
    {
        public string Text { get; set; }
        public IFormFile Picture { get; set; }
        public IFormFile File { get; set; }

        public bool ContainsPicture()
        {
            return Picture != null;
        }

        public bool ContainsFile()
        {
            return File != null;
        }
    }
}
