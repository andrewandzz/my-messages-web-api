using AutoMapper;
using Moq;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Services;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Services
{
    public class AccountServiceTests
    {
        private static readonly IMapper mapper;

        static AccountServiceTests()
        {
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(Logics.Mapping.MapperProfile))
            ).CreateMapper();
        }

        public class AuthenticateAsyncMethod
        {
            [Fact]
            public async Task WhenValidNameAndPasswordHashProvided_ReturnsUserDto()
            {
                // Arrange
                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByNameAndPasswordHashAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(new User()));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);

                var service = new AccountService(uow.Object, mapper);

                // Act
                var user = await service.AuthenticateAsync("name", "password");

                // Assert
                Assert.NotNull(user);
                Assert.IsType<UserDto>(user);
            }

            [Fact]
            public async Task WhenInvalidNameOrPasswordHashProvided_ThrowsNotFoundException()
            {
                // Arrange
                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByNameAndPasswordHashAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult<User>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);

                var service = new AccountService(uow.Object, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.AuthenticateAsync("random name", "random password"));
            }
        }
    }
}
