using Microsoft.EntityFrameworkCore;
using MyMessages.Data;
using MyMessages.Data.Entities;
using MyMessages.Data.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Repositories
{
    public class FileRepositoryTests
    {
        public class FindByIdAsyncMethod : IDisposable
        {
            private readonly AppDbContext context;
            private readonly FileRepository repository;

            public FindByIdAsyncMethod()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("FilesById").Options;
                context = new AppDbContext(options);
                repository = new FileRepository(context);
            }

            [Fact]
            public async Task WhenValidIdProvidedWithIncludes_ReturnsFileWithIncludes()
            {
                // Arrange
                context.Messages.Add(new Message() { File = new File() });
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var file = await repository.FindByIdAsync(1, file => file.Message);

                // Assert
                Assert.Equal(1, file.Id);
                Assert.NotNull(file.Message);
            }

            [Fact]
            public async Task WhenNoIncludesSpecified_ReturnsFileWithNoIncludes()
            {
                // Arrange
                context.Messages.Add(new Message() { File = new File() });
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var file = await repository.FindByIdAsync(1);

                // Assert
                Assert.Null(file.Message);
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
