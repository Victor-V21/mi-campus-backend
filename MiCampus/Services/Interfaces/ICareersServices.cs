using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;

namespace MiCampus.Services.Interfaces
{
    public interface ICareersServices
    {
        Task<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0
        );

        Task<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>> GetListAsync(
            string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        );

        Task<ResponseDto<CareerDto>> GetAllByIdAsync(string id);
        Task<ResponseDto<CareerDto>> GetByIdAsync(string id);
        Task<ResponseDto<CareerDto>> CreateAsync(CareerCreateDto dto);
        Task<ResponseDto<CareerDto>> EditAsync(string id, CareerEditDto dto);
    }
}