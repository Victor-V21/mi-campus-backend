
using Mapster;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Feedback;
using MiCampus.Dtos.Notification;
using MiCampus.Dtos.NotificationTypes;
using MiCampus.Dtos.Publication;
using MiCampus.Dtos.PublicationImage;
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
            
            TypeAdapterConfig<PublicationEntity, PublicationDto>.NewConfig();
            TypeAdapterConfig<PublicationEditDto, PublicationEntity>.NewConfig();

            // PublicationImage

            TypeAdapterConfig<PublicationImageEntity, PublicationImageDto>.NewConfig()
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.FileName, src => src.FileName)
              .Map(dest => dest.Url, src => src.Url)
              .Map(dest => dest.DateUpload, src => src.DateUpload);


            // Feedback
            TypeAdapterConfig<FeedbackEntity, FeedbackDto>.NewConfig();
           TypeAdapterConfig<FeekbackCreateDto, FeedbackEntity>.NewConfig();



            //Publication Type 
            TypeAdapterConfig<PublicationTypeCreateDto, PublicationTypeEntity>.NewConfig();
            TypeAdapterConfig<PublicationTypeEditDto, PublicationTypeEntity>.NewConfig();
            TypeAdapterConfig<PublicationTypeEntity, PublicationTypeDto>.NewConfig();


            //Notification
            TypeAdapterConfig<NotificationEntity, NotificationCreateDto>.NewConfig(); 
            TypeAdapterConfig<NotificationCreateDto, NotificationEntity>.NewConfig();

            //Notification Type
            TypeAdapterConfig<NotificationTypeCreateDto, NotificationTypeEntity>.NewConfig();
            TypeAdapterConfig<NotificationTypeEditDto, NotificationTypeEntity>.NewConfig();
            TypeAdapterConfig<NotificationTypeEntity, NotificationTypeDto>.NewConfig();

        }
    }
}