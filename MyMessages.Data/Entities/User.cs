using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MyMessages.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Message> Messages { get; set; }

        public void HashPassword(string password)
        {
            using var md5 = MD5.Create();

            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var hashBytes = md5.ComputeHash(passwordBytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            PasswordHash = sb.ToString();
        }
    }
}
