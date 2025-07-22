using AutoMapper;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Security.Roles;
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

            // Roles mappers
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<RoleEntity, RoleActionResponseDto>();
            CreateMap<RoleCreateDto, RoleEntity>();
            CreateMap<RoleEditDto, RoleEntity>();

            //Campus mappers

            //CreateMap<CampusesActionResponseDto, CampusEntity>().ReverseMap();
            //CreateMap<CampusesDto, CampusEntity>().ReverseMap();
            //CreateMap<CampusesCreateDto, CampusEntity>().ReverseMap();
            //CreateMap<CampusesEditDto, CampusEntity>();

        }
    }
}
