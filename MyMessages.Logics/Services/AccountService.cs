using AutoMapper;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System.Threading.Tasks;

namespace MyMessages.Logics.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<UserDto> AuthenticateAsync(string name, string passwordHash)
        {
            var user = await uow.Users.FindByNameAndPasswordHashAsync(name, passwordHash);

            if (user == null)
            {
                throw new NotFoundException();
            }

            return mapper.Map<UserDto>(user);
        }

        public async Task RegisterAsync(string name, string password)
        {
            var user = await uow.Users.FindByName(name);

            if (user != null)
            {
                throw new ConflictException($"User with name '{name}' already exists.");
            }

            var newUser = new User()
            {
                Name = name
            };

            newUser.HashPassword(password);

            uow.Users.Add(newUser);
            await uow.CommitAsync();
        }
    }
}
