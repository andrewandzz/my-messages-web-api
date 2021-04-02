using MyMessages.Logics.Dtos;
using System.Threading.Tasks;

namespace MyMessages.Logics.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> AuthenticateAsync(string name, string passwordHash);
        Task RegisterAsync(string name, string password);
    }
}
