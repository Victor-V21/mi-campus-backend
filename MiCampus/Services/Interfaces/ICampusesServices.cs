using System.Collections.Generic;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Common;

namespace MiCampus.Services.Interfaces
{
    public interface ICampusesServices
    {
        Task<ResponseDto<CampusDto>> CreateAsync(CampusCreateDto dto);
        Task<ResponseDto<PaginationDto<List<CampusActionResponseDto>>>> GetEnabledListAsync(
                string seachTerm = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<PaginationDto<List<CampusDto>>>> GetListAllAsync(
                string seachTerm = "", string isEnabled = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<CampusDto>> EditAsync(string id, CampusEditDto dto);
        Task<ResponseDto<CampusDto>> GetOneByIdAsync(string id);
    }
}