using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMessages.Data.Entities
{
    public class Sticker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
