using AutoMapper;
using MyMessages.Data.Entities;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMessages.Logics.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork uow;
        private readonly IFileService fileService;
        private readonly IMapper mapper;

        public MessageService(IUnitOfWork uow, IFileService fileService, IMapper mapper)
        {
            this.uow = uow;
            this.fileService = fileService;
            this.mapper = mapper;
        }

        public async Task<MessageDto> AddAsync(NewMessageDto dto, int userId)
        {
            var user = await uow.Users.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} not found.");
            }

            if (dto.StickerId != null)
            {
                var sticker = await uow.Stickers.FindByIdAsync(dto.StickerId.Value);

                if (sticker == null)
                {
                    throw new NotFoundException($"Sticker with id {dto.StickerId} not found.");
                }
            }

            var message = new Message()
            {
                Text = dto.Text,
                UserId = user.Id,
                StickerId = dto.StickerId,
                Date = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()
            };

            if (dto.ContainsPicture())
            {
                await AddPictureToMessageAndToServer(dto, message);
            }

            if (dto.ContainsFile())
            {
                await AddFileToMessageAndToServer(dto, message);
            }

            uow.Messages.Add(message);

            await uow.CommitAsync();

            var createdMessageDto = mapper.Map<MessageDto>(message);
            return createdMessageDto;
        }

        public async Task<MessageDto> EditAsync(int id, EditMessageDto dto, int userId)
        {
            var message = await uow.Messages.FindByIdAsync(id, message => message.Picture, message => message.File);

            if (message == null)
            {
                throw new NotFoundException($"Message with id {id} not found.");
            }

            if (!message.BelongsToUser(userId))
            {
                throw new ForbiddenException();
            }

            if (!message.IsAllowedToEdit())
            {
                throw new InvalidOperationException();
            }

            message.Text = dto.Text;

            if (message.ContainsPicture())
            {
                DeletePictureFromMessageAndFromServer(message.Picture);
            }

            if (message.ContainsFile())
            {
                DeleteFileFromMessageAndFromServer(message.File);
            }

            if (dto.ContainsPicture())
            {
                await AddPictureToMessageAndToServer(dto, message);
            }

            if (dto.ContainsFile())
            {
                await AddFileToMessageAndToServer(dto, message);
            }

            uow.Messages.Update(message);
            await uow.CommitAsync();

            var editedMessageDto = mapper.Map<MessageDto>(message);
            return editedMessageDto;
        }

        public async Task<MessagesDataDto> GetAllAsync(int userId, int? fromId, int? count)
        {
            var user = await uow.Users.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} not found.");
            }

            if (count != null && count <= 0)
            {
                throw new BadRequestException($"Value for 'count' parameter cannot be less or equal to 0. You passed {count.Value}.");
            }

            // TODO: refactor
            int? messagesCount = count;

            if (count != null)
            {
                // we need to fetch one more
                // to be able to get the next message's id
                messagesCount = count.Value + 1;
            }

            var messages = await uow.Messages.FindAllAsync(userId, fromId, messagesCount,
                message => message.Picture, message => message.File, message => message.Sticker);

            List<Message> requestedMessages = (count != null) ? messages.Take(count.Value).ToList() : messages;
            int? nextMessageId = (count != null) ? messages.Skip(count.Value).Take(1).FirstOrDefault()?.Id : null;

            var messagesDataDto = new MessagesDataDto()
            {
                Messages = mapper.Map<List<MessageDto>>(requestedMessages),
                NextId = nextMessageId
            };

            return messagesDataDto;
        }

        public async Task RemoveAsync(int id, int userId)
        {
            var message = await uow.Messages.FindByIdAsync(id, message => message.Picture, message => message.File);

            if (message == null)
            {
                throw new NotFoundException($"Message with id {id} not found.");
            }

            if (!message.BelongsToUser(userId))
            {
                throw new ForbiddenException();
            }

            if (!message.IsAllowedToEdit())
            {
                throw new InvalidOperationException();
            }

            if (message.ContainsPicture())
            {
                DeletePictureFromMessageAndFromServer(message.Picture);
            }

            if (message.ContainsFile())
            {
                DeleteFileFromMessageAndFromServer(message.File);
            }

            uow.Messages.Remove(message);
            await uow.CommitAsync();
        }

        private async Task AddPictureToMessageAndToServer(IncomingMessageDto dto, Message targetMessage)
        {
            var picture = new Picture()
            {
                Name = dto.Picture.FileName,
                Path = await fileService.UploadPictureAsync(dto.Picture)
            };

            targetMessage.Picture = picture;
        }

        private async Task AddFileToMessageAndToServer(IncomingMessageDto dto, Message targetMessage)
        {
            var file = new File()
            {
                Name = dto.File.FileName,
                Path = await fileService.UploadFileAsync(dto.File)
            };

            targetMessage.File = file;
        }

        private void DeletePictureFromMessageAndFromServer(Picture picture)
        {
            fileService.RemoveFile(picture.Path);
            uow.Pictures.Remove(picture);
        }

        private void DeleteFileFromMessageAndFromServer(File file)
        {
            fileService.RemoveFile(file.Path);
            uow.Files.Remove(file);
        }
    }
}
