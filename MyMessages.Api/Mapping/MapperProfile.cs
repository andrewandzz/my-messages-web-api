using AutoMapper;
using MyMessages.Api.Models;
using MyMessages.Logics.Dtos;

namespace MyMessages.Api.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MessageModel, MessageDto>().ReverseMap();
            CreateMap<MessagesDataModel, MessagesDataDto>().ReverseMap();
            CreateMap<NewMessageModel, NewMessageDto>();
            CreateMap<PictureDto, PictureModel>();
            CreateMap<FileDto, FileModel>();
            CreateMap<EditMessageModel, EditMessageDto>();
            CreateMap<StickerDto, StickerModel>();
        }
    }
}
