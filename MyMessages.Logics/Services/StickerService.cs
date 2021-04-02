using AutoMapper;
using MyMessages.Data.Interfaces;
using MyMessages.Logics.Dtos;
using MyMessages.Logics.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMessages.Logics.Services
{
    public class StickerService : IStickerService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public StickerService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<StickerDto>> GetAllAsync()
        {
            var stickers = await uow.Stickers.FindAllAsync();
            return mapper.Map<List<StickerDto>>(stickers);
        }
    }
}
