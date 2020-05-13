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
        }
    }
}