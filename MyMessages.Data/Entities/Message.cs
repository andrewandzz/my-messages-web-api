using System;
using System.ComponentModel.DataAnnotations;

namespace MyMessages.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public string Text { get; set; }

        public Picture Picture { get; set; }
        public int? PictureId { get; set; }

        public File File { get; set; }
        public int? FileId { get; set; }

        public Sticker Sticker { get; set; }
        public int? StickerId { get; set; }

        [Required]
        public long? Date { get; set; }

        private const int AllowedEditTime = 15; // in minutes

        public bool BelongsToUser(int userId)
        {
            return UserId == userId;
        }

        public bool IsAllowedToEdit()
        {
            var nowUtc = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
            var elapsedTime = (nowUtc - Date) / 1000.0 / 60.0;
            return (elapsedTime < AllowedEditTime);
        }

        public bool ContainsPicture()
        {
            return PictureId != null;
        }

        public bool ContainsFile()
        {
            return FileId != null;
        }
    }
}
