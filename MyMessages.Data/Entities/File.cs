using System.ComponentModel.DataAnnotations;

namespace MyMessages.Data.Entities
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public Message Message { get; set; }
        public int MessageId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        public bool BelongsToUser(int userId)
        {
            return Message.UserId == userId;
        }
    }
}
