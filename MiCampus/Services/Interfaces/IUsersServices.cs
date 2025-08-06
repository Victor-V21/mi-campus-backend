using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Users;

namespace MiCampus.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<ResponseDto<UserActionResponseDto>> CreateAsync(UserCreateDto dto);
        Task<ResponseDto<UserActionResponseDto>> DeleteAsync(string id);
        Task<ResponseDto<UserActionResponseDto>> EditAsync(UserEditDto dto, string id);
        Task<ResponseDto<PaginationDto<List<UserDto>>>> GetListAsync(string seachTerm = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<UserDto>> GetOneByIdAsync(string id);
        Task<ResponseDto<UserActionResponseDto>> RegisterAsync(UserCreateDto dto);
    }
}
