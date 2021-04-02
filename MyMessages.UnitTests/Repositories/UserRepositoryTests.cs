using Microsoft.EntityFrameworkCore;
using MyMessages.Data;
using MyMessages.Data.Entities;
using MyMessages.Data.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        public class FindByNameAndPasswordHashAsyncMethod : IDisposable
        {
            private readonly AppDbContext context;
            private readonly UserRepository repository;

            public FindByNameAndPasswordHashAsyncMethod()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("Users").Options;
                context = new AppDbContext(options);
                repository = new UserRepository(context);
            }

            [Fact]
            public async Task WhenValidArgumentsProvided_ReturnsCorrespondingUser()
            {
                // Arrange
                const string Name = "UserName";
                const string PasswordHash = "Password";

                context.Users.AddRange(
                    new User() { Name = Name, PasswordHash = "123" },
                    new User() { Name = Name, PasswordHash = PasswordHash },
                    new User() { Name = Name + " ", PasswordHash = "456" }
                );
                await context.SaveChangesAsync();

                // Act
                var user = await repository.FindByNameAndPasswordHashAsync(" " + Name, PasswordHash);

                // Assert
                Assert.NotNull(user);
                Assert.Equal(2, user.Id);
                Assert.Equal(Name, user.Name);
                Assert.Equal(PasswordHash, user.PasswordHash);
            }

            public void Dispose()
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}
