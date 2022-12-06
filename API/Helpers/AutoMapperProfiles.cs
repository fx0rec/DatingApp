using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        //The idea of AutoMapper, is that it takes away the writing of tedious code.
        public AutoMapperProfiles() 
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.
            MapFrom(src => src.Photos.FirstOrDefault(x=>x.IsMain).Url)); 
            //Gets the Photo Url from AppUser.Photo and sets it to PhotoUrl string in MemberDto. 


            CreateMap<Photo, PhotoDto>();   //Maps Photo to PhotoDto
            //It simply maps the properties from one to another, and is smart enough to infer that the int Age = GetAge()
            //It needs to be injected into the controller, so it needs a service.



        }
    }
}