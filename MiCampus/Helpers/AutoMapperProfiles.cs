using AutoMapper;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Security.Users;

namespace MiCampus.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            // User mappers
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserEntity, UserActionResponseDto>()
                .ForMember(dest => dest.FullName, org => org.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<UserCreateDto, UserEntity>()
                .ForMember(dest => dest.UserName, org => org.MapFrom(src => src.Email));
            CreateMap<UserEditDto, UserEntity>();
        }

    }
}
