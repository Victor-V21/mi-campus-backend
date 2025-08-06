
using Mapster;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Notification;
using MiCampus.Dtos.NotificationTypes;
using MiCampus.Dtos.Publication;
using MiCampus.Dtos.PublicationTypes;
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

            TypeAdapterConfig<CampusEntity, CampusCreateDto>.NewConfig();
            TypeAdapterConfig<CampusCreateDto, CampusEntity>.NewConfig();

            //Publication
            TypeAdapterConfig<PublicationEntity, PublicationCreateDto>.NewConfig();
            TypeAdapterConfig<PublicationCreateDto, PublicationEntity>.NewConfig();

            //Publication Type 
            TypeAdapterConfig<PublicationTypeEntity, PublicationTypeCreateDto>.NewConfig();
            TypeAdapterConfig<PublicationTypeCreateDto, PublicationEntity>.NewConfig();

            //Notification
            TypeAdapterConfig<NotificationEntity, NotificationCreateDto>.NewConfig(); 
            TypeAdapterConfig<NotificationCreateDto, NotificationEntity>.NewConfig();

            //Notification Type
            TypeAdapterConfig<NotificationTypeEntity, NotificationTypeCreateDto>.NewConfig();
            TypeAdapterConfig<NotificationTypeCreateDto, NotificationTypeEntity>.NewConfig();
        }
    }
}