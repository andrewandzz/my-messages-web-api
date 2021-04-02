using MyMessages.Data.Interfaces;
using System.Threading.Tasks;

namespace MyMessages.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext db;

        public IUserRepository Users { get; }
        public IMessageRepository Messages { get; }
        public IPictureRepository Pictures { get; }
        public IFileRepository Files { get; }
        public IStickerRepository Stickers { get; set; }

        public UnitOfWork(
            AppDbContext db,
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IPictureRepository pictureRepository,
            IFileRepository fileRepository,
            IStickerRepository stickerRepository
        )
        {
            this.db = db;
            Users = userRepository;
            Messages = messageRepository;
            Pictures = pictureRepository;
            Files = fileRepository;
            Stickers = stickerRepository;
        }

        public Task CommitAsync()
        {
            return db.SaveChangesAsync();
        }
    }
}
