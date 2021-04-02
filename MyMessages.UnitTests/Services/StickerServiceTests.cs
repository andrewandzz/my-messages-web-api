using AutoMapper;
using Moq;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Services
{
    public class StickerServiceTests
    {
        private static readonly IMapper mapper;

        static StickerServiceTests()
        {
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(Logics.Mapping.MapperProfile))
            ).CreateMapper();
        }

        public class GetAllAsyncMethod
        {
            [Fact]
            public async Task ReturnsListOfAllStickerDtos()
            {
                // Arrange
                var stickers = new List<Sticker>()
                {
                    new Sticker(),
                    new Sticker(),
                    new Sticker()
                };

                var stickerRepository = new Mock<IStickerRepository>();
                stickerRepository.Setup(repo => repo.FindAllAsync()).Returns(Task.FromResult(stickers));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Stickers).Returns(stickerRepository.Object);

                var stickerService = new StickerService(uow.Object, mapper);

                // Act
                var stickerDtos = await stickerService.GetAllAsync();

                // Assert
                Assert.NotNull(stickerDtos);
                Assert.Equal(3, stickerDtos.Count);
                Assert.All(stickerDtos, stickerDto => Assert.IsType<StickerDto>(stickerDto));
            }
        }
    }
}
