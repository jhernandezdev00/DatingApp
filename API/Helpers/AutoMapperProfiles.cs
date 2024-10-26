using API.DataEntities;
using API.DTOs;
using AutoMapper;
namespace API.Helpers;

public class AutoMapperProfiles : Profile{
    public AutoMapperProfiles(){
        CreateMap<AppUser, MemberResponse>().ForMember(
            d => d.PhotoUrl, 
            o => o.MapFrom(
                s => s.Photos.FirstOrDefault(
                    p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
    }
}