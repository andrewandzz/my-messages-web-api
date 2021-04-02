using Microsoft.EntityFrameworkCore;
using MyMessages.Data;
using MyMessages.Data.Entities;
using MyMessages.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Repositories
{
    public class MessageRepositoryTests
    {
        public class FindAllAsyncMethod : IDisposable
        {
            private readonly AppDbContext context;
            private readonly MessageRepository repository;

            public FindAllAsyncMethod()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("MessagesAll").Options;
                context = new AppDbContext(options);
                repository = new MessageRepository(context);
            }

            [Fact]
            public async Task WhenValidArgumentsProvidedWithIncludes_ReturnsListOfMessagesWithIncludes()
            {
                // Arrange
                context.Users.AddRange(
                    new User(),
                    new User()
                );
                await context.SaveChangesAsync();

                context.Messages.AddRange(
                    new Message() { UserId = 1, Picture = new Picture(), File = new File(), },
                    new Message() { UserId = 1, Picture = new Picture(), File = new File(), },
                    new Message() { UserId = 1, Picture = new Picture(), File = new File(), },
                    new Message() { UserId = 1, Picture = new Picture(), File = new File(), },
                    new Message() { UserId = 2, Picture = new Picture(), File = new File(), }
                );
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var messages = await repository.FindAllAsync(1, 3, 2, message => message.Picture, message => message.File);

                // Assert
                Assert.Equal(2, messages.Count);
                Assert.All(messages, message => Assert.Equal(1, message.UserId));
                Assert.Equal(3, messages.First().Id);
                Assert.All(messages, message => Assert.NotNull(message.Picture));
                Assert.All(messages, message => Assert.NotNull(message.File));
            }

            [Fact]
            public async Task WhenFromIdAndCountAreNotSpecified_ReturnsListOfAllMessagesForTheCorrespondingUser()
            {
                // Arrange
                context.Users.AddRange(
                    new User(),
                    new User()
                );
                await context.SaveChangesAsync();

                context.Messages.AddRange(
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 2 }
                );
                await context.SaveChangesAsync();

                // Act
                var messages = await repository.FindAllAsync(1, null, null);

                // Assert
                Assert.Equal(4, messages.Count);
            }

            [Fact]
            public async Task WhenFromIdIsNotSpecified_ReturnsListOfMessagesFromTheVeryLast()
            {
                // Arrange
                context.Users.Add(new User());
                await context.SaveChangesAsync();

                context.Messages.AddRange(
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 2 }
                );
                await context.SaveChangesAsync();

                // Act
                var messages = await repository.FindAllAsync(1, null, 2);

                // Assert
                Assert.Equal(4, messages.First().Id);
                Assert.Equal(2, messages.Count);
            }

            [Fact]
            public async Task WhenCountIsNotSpecified_ReturnsListOfMessagesFromSpecifiedIdToTheVeryEnd()
            {
                // Arrange
                context.Users.Add(new User());
                await context.SaveChangesAsync();

                context.Messages.AddRange(
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 },
                    new Message() { UserId = 1 }
                );
                await context.SaveChangesAsync();

                // Act
                var messages = await repository.FindAllAsync(1, 3, null);

                // Assert
                Assert.Equal(3, messages.Count);
                Assert.Equal(1, messages.Last().Id);
            }

            [Fact]
            public async Task WhenZeroOrNegativeCountProvided_ThrowsArgumentException()
            {
                // Arrange

                // Act and Assert
                await Assert.ThrowsAsync<ArgumentException>(() => repository.FindAllAsync(1, 10, 0));
                await Assert.ThrowsAsync<ArgumentException>(() => repository.FindAllAsync(1, 10, -5));
            }

            [Fact]
            public async Task WhenNoIncludesSpecified_ReturnsMessagesWithNoIncludes()
            {
                // Arrange
                context.Users.Add(new User());
                await context.SaveChangesAsync();

                context.Messages.Add(new Message() { UserId = 1, Picture = new Picture(), File = new File() });
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var messages = await repository.FindAllAsync(1, null, null);

                // Assert
                Assert.All(messages, message => Assert.Null(message.Picture));
                Assert.All(messages, message => Assert.Null(message.File));
            }

            public void Dispose()
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        public class FindByIdAsyncMethod : IDisposable
        {
            private readonly AppDbContext context;
            private readonly MessageRepository repository;

            public FindByIdAsyncMethod()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("MessagesById").Options;
                context = new AppDbContext(options);
                repository = new MessageRepository(context);
            }

            [Fact]
            public async Task WhenValidArgumentsProvidedWithIncludes_ReturnsMessageWithIncludes()
            {
                // Arrange
                const int Id = 2;

                context.Messages.AddRange(
                    new Message() { Picture = new Picture(), File = new File() },
                    new Message() { Picture = new Picture(), File = new File() }
                );
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var message = await repository.FindByIdAsync(Id, message => message.Picture, message => message.File);

                // Assert
                Assert.Equal(Id, message.Id);
                Assert.NotNull(message.Picture);
                Assert.NotNull(message.File);
            }

            [Fact]
            public async Task WhenNoIncludesSpecified_ReturnsMessageWithNoIncludes()
            {
                // Arrange
                const int Id = 1;

                context.Messages.AddRange(
                    new Message() { Picture = new Picture(), File = new File() },
                    new Message() { Picture = new Picture(), File = new File() }
                );
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var message = await repository.FindByIdAsync(Id);

                // Assert
                Assert.Null(message.Picture);
                Assert.Null(message.File);
            }

            public void Dispose()
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        private static void DetachAllEntries(AppDbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
