using System.Threading.Tasks;

namespace MyMessages.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; }
        public IMessageRepository Messages { get; }
        public IPictureRepository Pictures { get; }
        public IFileRepository Files { get; }
        public IStickerRepository Stickers { get; set; }

        Task CommitAsync();
    }
}
