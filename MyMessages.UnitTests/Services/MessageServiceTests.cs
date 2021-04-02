using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using MyMessages.Logics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyMessages.UnitTests.Services
{
    public class MessageServiceTests
    {
        private static readonly IFileService fileService;
        private static readonly IMapper mapper;

        static MessageServiceTests()
        {
            fileService = new Mock<IFileService>().Object;
            mapper = new MapperConfiguration(config =>
                config.AddProfile(typeof(Logics.Mapping.MapperProfile))
            ).CreateMapper();
        }

        public class AddAsyncMethod
        {
            [Fact]
            public async Task WhenValidDtoAndUserIdProvided_ReturnsMessageDto()
            {
                // Arrange
                var user = new User() { Id = 1 };
                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(user));

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.Add(It.IsAny<Message>()));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var newMessageDto = new NewMessageDto()
                {
                    Text = "text",
                    Picture = new Mock<IFormFile>().Object,
                    File = new Mock<IFormFile>().Object
                };

                // Act
                var createdMessageDto = await service.AddAsync(newMessageDto, 1);

                // Assert
                Assert.NotNull(createdMessageDto);
                Assert.IsType<MessageDto>(createdMessageDto);
            }

            [Fact]
            public async Task WhenValidDtoButInvalidUserIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<User>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var newMessageDto = new NewMessageDto()
                {
                    Text = "text",
                    Picture = new Mock<IFormFile>().Object,
                    File = new Mock<IFormFile>().Object
                };

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.AddAsync(newMessageDto, 1));
            }
        }

        public class EditAsyncMethod
        {
            [Fact]
            public async Task WhenValidArgumentsProvided_ReturnsEditedMessageDto()
            {
                // Arrange
                const string NewText = "NewText";
                const int UserId = 1;

                var date = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(-1)).ToUnixTimeMilliseconds();
                var message = new Message() { UserId = UserId, Text = "Initial text", Date = date };

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(message));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var dto = new EditMessageDto()
                {
                    Text = NewText
                };

                // Act
                var editedMessageDto = await service.EditAsync(1, dto, UserId);

                // Assert
                Assert.NotNull(editedMessageDto);
                Assert.Equal(NewText, editedMessageDto.Text);
                Assert.IsType<MessageDto>(editedMessageDto);
            }

            [Fact]
            public async Task WhenInvalidIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                     .Setup(repo => repo.FindByIdAsync(
                         It.IsAny<int>(),
                         It.IsAny<Expression<Func<Message, object>>>(),
                         It.IsAny<Expression<Func<Message, object>>>())
                     ).Returns(Task.FromResult<Message>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var dto = new EditMessageDto();

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.EditAsync(1, dto, 1));
            }

            [Fact]
            public async Task WhenValidIdButInvalidUserIdProvided_ThrowsForbiddenException()
            {
                // Arrange
                var message = new Message() { UserId = 1 };

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(message));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var dto = new EditMessageDto();

                // Act and Assert
                await Assert.ThrowsAsync<ForbiddenException>(() => service.EditAsync(1, dto, 2));
            }

            [Fact]
            public async Task WhenValidIdAndUserIdButTimeToEditIsOut_ThrowsInvalidOperationException()
            {
                // Arrange
                const int UserId = 1;

                var date = ((DateTimeOffset)DateTime.UtcNow.AddHours(-1)).ToUnixTimeMilliseconds();
                var message = new Message() { UserId = UserId, Date = date };

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(message));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                var dto = new EditMessageDto();

                // Act and Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => service.EditAsync(1, dto, UserId));
            }
        }

        public class GetAllAsyncMethod
        {
            [Fact]
            public async Task WhenValidArgumentsProvided_ReturnsListOfMessageDataDto()
            {
                // Arrange
                const int FromId = 5;
                const int MessagesCount = 3;

                var user = new User();

                // this result we have coming from the repository
                var messages = new List<Message>()
                {
                    new Message() { Id = 5 },
                    new Message() { Id = 4 },
                    new Message() { Id = 3 },
                    new Message() { Id = 1 }
                };

                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(user));

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindAllAsync(
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(messages));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act
                var messageDataDto = await service.GetAllAsync(1, FromId, MessagesCount);

                // Assert
                Assert.NotNull(messageDataDto);
                Assert.IsType<MessagesDataDto>(messageDataDto);
                Assert.Equal(MessagesCount, messageDataDto.Messages.Count);
                Assert.Equal(FromId, messageDataDto.Messages.First().Id);
                Assert.Equal(1, messageDataDto.NextId);
            }

            [Fact]
            public async Task WhenInvalidUserIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<User>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.GetAllAsync(1, 1, 1));
            }

            [Fact]
            public async Task WhenValidUserIdButCountIsZeroOrNegative_ThrowsBadRequestException()
            {
                // Arrange
                var user = new User();

                var userRepository = new Mock<IUserRepository>();
                userRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(user));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Users).Returns(userRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<BadRequestException>(() => service.GetAllAsync(1, null, 0));
                await Assert.ThrowsAsync<BadRequestException>(() => service.GetAllAsync(1, null, -1));
            }
        }

        public class RemoveAsyncMethod
        {
            [Fact]
            public async Task WhenInvalidIdProvided_ThrowsNotFoundException()
            {
                // Arrange
                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult<Message>(null));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveAsync(1, 1));
            }

            [Fact]
            public async Task WhenValidIdButInvalidUserIdProvided_ThrowsForbiddenException()
            {
                // Arrange
                var message = new Message() { UserId = 1 };

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(message));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<ForbiddenException>(() => service.RemoveAsync(1, 2));
            }

            [Fact]
            public async Task WhenValidArgumentsProvidedButTimeToEditIsOut_ThrowsInvalidOperationException()
            {
                // Arrange
                var date = ((DateTimeOffset)DateTime.UtcNow.AddHours(-1)).ToUnixTimeMilliseconds();
                var message = new Message() { UserId = 1, Date = date };

                var messageRepository = new Mock<IMessageRepository>();
                messageRepository
                    .Setup(repo => repo.FindByIdAsync(
                        It.IsAny<int>(),
                        It.IsAny<Expression<Func<Message, object>>>(),
                        It.IsAny<Expression<Func<Message, object>>>())
                    ).Returns(Task.FromResult(message));

                var uow = new Mock<IUnitOfWork>();
                uow.SetupGet(uow => uow.Messages).Returns(messageRepository.Object);

                var service = new MessageService(uow.Object, fileService, mapper);

                // Act and Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemoveAsync(1, 1));
            }
        }
    }
}
