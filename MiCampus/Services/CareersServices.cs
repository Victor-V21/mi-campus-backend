
using MiCampus.Database;
using MiCampus.Dtos.Common;

namespace MiCampus.Services
{
    public class CareersServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public CareersServices(
            CampusDbContext context,
            IConfiguration configuration
        )
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }
        /*
            PRIMERO HACER TABLA Y SERVICIO DE TYPOS DE GRADES
        */
        // public async Task<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>> GetEnabledListAsync(
        //     string searchTerm = "", int page = 1, int pageSize = 0
        // )
        // {
        //     pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

        //     int startIndex = (page - 1) * pageSize;

        //     IQueryable<CareerEntity> careerQuery = _context.Careers.Where(x => x.IsEnabled);

        //     if (!string.IsNullOrEmpty(searchTerm))
        //     {
        //         careerQuery = careerQuery
        //             .Where(x => (x.Name + " " + x.Description)
        //             .Contains(searchTerm));
        //     }

        //     int totalRows = await careerQuery.CountAsync();

        //     var careersEntity = await careerQuery
        //         .OrderBy(x => x.Name)
        //         .Skip(startIndex)
        //         .Take(pageSize)
        //         .ToListAsync();

        //     var careersDtos = careersEntity.Adapt<List<CareerActionResponseDto>>();

        //     return new ResponseDto<PaginationDto<List<CareerActionResponseDto>>>
        //     {
        //         StatusCode = HttpStatusCode.OK,
        //         Data = new PaginationDto<List<CareerActionResponseDto>>
        //         {
        //             Items = careersDtos,
        //             TotalRows = totalRows,
        //             PageSize = pageSize,
        //             CurrentPage = page
        //         }
        //     };
        // }
    }
}