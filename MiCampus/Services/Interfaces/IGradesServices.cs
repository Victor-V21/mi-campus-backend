using MiCampus.Dtos.Common;
using MiCampus.Dtos.Grades;

namespace MiCampus.Services.Interfaces
{
    public interface IGradesServices
    {
        Task<ResponseDto<PaginationDto<List<GradeDto>>>> GetListAsync(
            string seachTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        );

        Task<ResponseDto<GradeDto>> CreateAsync(GradeCreateDto dto);
        Task<ResponseDto<GradeDto>> UpdateAsync(string id, GradeCreateDto dto);
    }
}