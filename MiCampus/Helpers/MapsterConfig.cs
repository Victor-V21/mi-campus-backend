
using Mapster;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Security.Roles;
using MiCampus.Dtos.Security.Users;

namespace MiCampus.Helpers
{
    public class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<UserEntity, UserDto>.NewConfig();

            TypeAdapterConfig<UserEntity, UserActionResponseDto>.NewConfig()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

            TypeAdapterConfig<UserCreateDto, UserEntity>.NewConfig()
                .Map(dest => dest.UserName, src => src.Email);

            TypeAdapterConfig<UserEditDto, UserEntity>.NewConfig();

            // Roles mappers
            TypeAdapterConfig<RoleEntity, RoleDto>.NewConfig();
            TypeAdapterConfig<RoleEntity, RoleActionResponseDto>.NewConfig();
            TypeAdapterConfig<RoleCreateDto, RoleEntity>.NewConfig();
            TypeAdapterConfig<RoleEditDto, RoleEntity>.NewConfig();

            //campus
        }
    }
}