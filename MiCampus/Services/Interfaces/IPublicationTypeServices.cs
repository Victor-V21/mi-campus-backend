using MiCampus.Dtos.Common;
using MiCampus.Dtos.PublicationTypes;

namespace MiCampus.Services.Interfaces
{
    public interface IPublicationTypeServices
    {
        Task<ResponseDto<PublicationTypeDto>> CreateAsync(PublicationTypeCreateDto dto);
        Task<ResponseDto<PublicationTypeDto>> DeleteAsync(string id);
        Task<ResponseDto<PublicationTypeDto>> EditAsync(string id, PublicationTypeEditDto dto);
        Task<ResponseDto<PaginationDto<List<PublicationTypeDto>>>> GetListAsync(string searchTerm = "", int page = 1, int pageSize = 0);
    }
}
