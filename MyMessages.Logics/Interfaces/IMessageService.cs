using MyMessages.Logics.Dtos;
using System.Threading.Tasks;

namespace MyMessages.Logics.Interfaces
{
    public interface IMessageService
    {
        Task<MessagesDataDto> GetAllAsync(int userId, int? fromId, int? count);
        Task<MessageDto> AddAsync(NewMessageDto dto, int userId);
        Task<MessageDto> EditAsync(int id, EditMessageDto dto, int userId);
        Task RemoveAsync(int id, int userId);
    }
}
