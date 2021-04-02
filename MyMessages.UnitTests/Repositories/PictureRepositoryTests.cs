using Microsoft.EntityFrameworkCore;
using MyMessages.Data;
using MyMessages.Data.Entities;
using MyMessages.Data.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Repositories
{
    public class PictureRepositoryTests
    {
        public class FindByIdAsyncMethod : IDisposable
        {
            private readonly AppDbContext context;
            private readonly PictureRepository repository;

            public FindByIdAsyncMethod()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("PicturesById").Options;
                context = new AppDbContext(options);
                repository = new PictureRepository(context);
            }

            [Fact]
            public async Task WhenValidIdProvidedWithIncludes_ReturnsFileWithIncludes()
            {
                // Arrange
                context.Messages.Add(new Message() { Picture = new Picture() });
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var picture = await repository.FindByIdAsync(1, picture => picture.Message);

                // Assert
                Assert.Equal(1, picture.Id);
                Assert.NotNull(picture.Message);
            }

            [Fact]
            public async Task WhenNoIncludesSpecified_ReturnsFileWithNoIncludes()
            {
                // Arrange
                context.Messages.Add(new Message() { Picture = new Picture() });
                await context.SaveChangesAsync();

                DetachAllEntries(context);

                // Act
                var picture = await repository.FindByIdAsync(1);

                // Assert
                Assert.Null(picture.Message);
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
