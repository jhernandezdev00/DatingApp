using API.DataEntities;
using API.DTOs;
using API.Extensions;
using AutoMapper;
<<<<<<< HEAD
using System.Globalization;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
=======
namespace API.Helpers;

public class AutoMapperProfiles : Profile{
    public AutoMapperProfiles(){
>>>>>>> main
        CreateMap<AppUser, MemberResponse>()
            .ForMember(d => d.Age,
                o => o.MapFrom(s => s.BirthDay.CalculateAge()))
            .ForMember(d => d.PhotoUrl,
                o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
        CreateMap<MemberUpdateRequest, AppUser>();
<<<<<<< HEAD
        CreateMap<RegisterRequest, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s, CultureInfo.InvariantCulture));
=======
>>>>>>> main
    }
}