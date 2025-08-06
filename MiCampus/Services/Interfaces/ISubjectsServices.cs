using MiCampus.Dtos.Common;
using MiCampus.Dtos.Subjects;

namespace MiCampus.Services.Interfaces
{
    public interface ISubjectsServices
    {
        Task<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0);

        Task<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>> GetAllAsync
                (string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<SubjectDto>> GetByIdAsync(string id);
        Task<ResponseDto<SubjectDto>> CreateAsync(SubjectCreateDto dto);
        Task<ResponseDto<SubjectDto>> UpdateAsync(string id, SubjectEditDto dto);
    }
}