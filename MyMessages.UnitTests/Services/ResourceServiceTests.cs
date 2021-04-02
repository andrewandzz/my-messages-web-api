using AutoMapper;
using Moq;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Services
{
    public class ResourceServiceTests
    {
        private static readonly IMapper mapper;

        static ResourceServiceTests()
        {
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(Logics.Mapping.MapperProfile))
            ).CreateMapper();
        }

        public class GetFileByIdAsyncMethod
        {
            [Fact]
            public async Task WhenValidIdAndUserIdProvided_ReturnsFileDtoWithCorrectContentType()
            {
                // Arrange
                var message = new Message() { UserId = 1 };
                var file = new File() { Path = "book.pdf", Message = message };

                var fileRepository = new Mock<IFileRepository>();
                fileRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<File, object>>>()))
                    .Returns(Task.FromResult(file));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Files).Returns(fileRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act
                var fileDto = await service.GetFileByIdAsync(1, 1);

                // Assert
                Assert.NotNull(fileDto);
                Assert.Equal("application/pdf", fileDto.ContentType);
                Assert.IsType<FileDto>(fileDto);
            }

            [Fact]
            public async Task WhenInvalidIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var fileRepository = new Mock<IFileRepository>();
                fileRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<File, object>>>()))
                    .Returns(Task.FromResult<File>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Files).Returns(fileRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetFileByIdAsync(1, 1));
            }

            [Fact]
            public async Task WhenValidIdButInvalidUserId_ThrowsForbiddenException()
            {
                // Arrange
                var message = new Message() { UserId = 1 };
                var file = new File() { Message = message };

                var fileRepository = new Mock<IFileRepository>();
                fileRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<File, object>>>()))
                    .Returns(Task.FromResult(file));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Files).Returns(fileRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<ForbiddenException>(() => service.GetFileByIdAsync(1, 2));
            }
        }

        public class GetPictureByIdAsyncMethod
        {
            [Fact]
            public async Task WhenValidIdAndUserIdProvided_ReturnsPictureDtoWithCorrectContentType()
            {
                // Arrange
                var message = new Message() { UserId = 1 };
                var picture = new Picture() { Path = "photo.png", Message = message };

                var pictureRepository = new Mock<IPictureRepository>();
                pictureRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Picture, object>>>()))
                    .Returns(Task.FromResult(picture));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Pictures).Returns(pictureRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act
                var pictureDto = await service.GetPictureByIdAsync(1, 1);

                // Assert
                Assert.NotNull(pictureDto);
                Assert.Equal("image/png", pictureDto.ContentType);
                Assert.IsType<PictureDto>(pictureDto);
            }

            [Fact]
            public async Task WhenInvalidIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var pictureRepository = new Mock<IPictureRepository>();
                pictureRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Picture, object>>>()))
                    .Returns(Task.FromResult<Picture>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Pictures).Returns(pictureRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetPictureByIdAsync(1, 1));
            }

            [Fact]
            public async Task WhenValidIdButInvalidUserId_ThrowsForbiddenException()
            {
                // Arrange
                var message = new Message() { UserId = 1 };
                var pictire = new Picture() { Message = message };

                var pictureRepository = new Mock<IPictureRepository>();
                pictureRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Picture, object>>>()))
                    .Returns(Task.FromResult(pictire));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Pictures).Returns(pictureRepository.Object);

                var service = new ResourceService(uow.Object, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<ForbiddenException>(() => service.GetPictureByIdAsync(1, 2));
            }
        }
    }
}
