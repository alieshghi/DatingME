using System.Linq;
using AutoMapper;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserForDetails>()
            .ForMember(des => des.PhotoUrl, opt=> opt.MapFrom(src=>
            src.Photos.FirstOrDefault(x=>x.IsMain).Url
            ))
            .ForMember(des => des.Age,opt => opt.MapFrom(src =>
            src.DateOfBirth.CalCauteAge()
            ));
            CreateMap<User,UserForListDto>()
            .ForMember(des=> des.PhotoUrl, opt=> opt.MapFrom(src=>
            src.Photos.FirstOrDefault(x=>x.IsMain).Url
            ))
            .ForMember(des => des.Age,opt => opt.MapFrom(src =>
            src.DateOfBirth.CalCauteAge()
            ));
            CreateMap<Photo,PhotoForDetails>();
            CreateMap<Photo,PhotoToReturnDto>();
            CreateMap<PhotosToCreatDto,Photo>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<UserForRegisterDto,User>();
            CreateMap<Message,MessageToReturnDto>().ForMember(x => x.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                                                   .ForMember(x => x.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<Message,MessageToCreateDto>().ReverseMap();
            
        }
    }
}