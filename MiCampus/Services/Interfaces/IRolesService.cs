using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Roles;

namespace MiCampus.Services.Interfaces
{
    public interface IRolesService
    {
        Task<ResponseDto<RoleActionResponseDto>> CreateAsync(RoleCreateDto dto);
        Task<ResponseDto<RoleActionResponseDto>> DeleteAsync(string id);
        Task<ResponseDto<RoleActionResponseDto>> EditAsync(RoleEditDto dto, string id);
        Task<ResponseDto<PaginationDto<List<RoleDto>>>> GetListAsync(string searchTerm = "", int page = 1, int pageSize = 10);
        Task<ResponseDto<RoleDto>> GetOneById(string id);
    }
}
