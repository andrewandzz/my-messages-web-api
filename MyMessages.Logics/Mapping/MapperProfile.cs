using AutoMapper;
using MyMessages.Data.Entities;
using MyMessages.Logics.Dtos;

namespace MyMessages.Logics.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<Picture, PictureDto>();
            CreateMap<File, FileDto>();
            CreateMap<Sticker, StickerDto>();
        }
    }
}
