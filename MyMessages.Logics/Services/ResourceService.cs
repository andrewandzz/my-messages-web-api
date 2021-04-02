using AutoMapper;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System.Threading.Tasks;

namespace MyMessages.Logics.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ResourceService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<FileDto> GetFileByIdAsync(int id, int userId)
        {
            var file = await uow.Files.FindByIdAsync(id, file => file.Message);

            if (file == null)
            {
                throw new NotFoundException($"File with id {id} not found.");
            }

            if (!file.BelongsToUser(userId))
            {
                throw new ForbiddenException();
            }

            var fileDto = mapper.Map<FileDto>(file);
            fileDto.UpdateContentType();

            return fileDto;
        }

        public async Task<PictureDto> GetPictureByIdAsync(int id, int userId)
        {
            var picture = await uow.Pictures.FindByIdAsync(id, picture => picture.Message);

            if (picture == null)
            {
                throw new NotFoundException($"Picture with id {id} not found.");
            }

            if (!picture.BelongsToUser(userId))
            {
                throw new ForbiddenException();
            }

            var pictureDto = mapper.Map<PictureDto>(picture);
            pictureDto.UpdateContentType();

            return pictureDto;
        }

        public async Task<StickerDto> GetStickerByIdAsync(int id)
        {
            var sticker = await uow.Stickers.FindByIdAsync(id);

            if (sticker == null)
            {
                throw new NotFoundException($"Sticker with id {id} not found.");
            }

            var stickerDto = mapper.Map<StickerDto>(sticker);
            stickerDto.UpdateContentType();

            return stickerDto;
        }
    }
}
