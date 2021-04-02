using Microsoft.AspNetCore.Http;

namespace MyMessages.Api.Models
{
    public class EditMessageModel
    {
        public string Text { get; set; }
        public IFormFile Picture { get; set; }
        public IFormFile File { get; set; }
    }
}
