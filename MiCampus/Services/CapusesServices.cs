using AutoMapper;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Roles;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class CapusesServices
    {
        private readonly CampusDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public CapusesServices(
            CampusDbContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            PAGE_SIZE = _configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = _configuration.GetValue<int>("PageSizeLimit");
        }

        // Servicio para listar los campus

        //public async Task<ResponseDto<PaginationDto<List<CampusesDto>>>> GetListAsync(
        //    string searchTerm = "", int page = 1, int pageSize = 10)
        //{
        //    pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;
        //    int startIndex = (page - 1) * pageSize;
        //    IQueryable<CampusEntity> campusesQuery = _context.Campuses.Include(c => c.Careers);

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        campusesQuery = campusesQuery
        //            .Where(c => (c.Name + " " + c.Description)
        //            .Contains(searchTerm));
        //    }

        //    int totalRows = await campusesQuery.CountAsync();

        //    var campuses = await campusesQuery
        //        .OrderBy(c => c.Name)
        //        .Skip(startIndex)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    var campusesDtos = _mapper.Map<List<CampusesDto>>(campuses);

        //    return new ResponseDto<PaginationDto<List<CampusesDto>>>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Status = true,
        //        Message = "Lista de campus obtenida correctamente.",
        //        Data = new PaginationDto<List<CampusesDto>>
        //        {
        //            CurrentPage = page,
        //            PageSize = pageSize,
        //            TotalItems = totalRows,
        //            TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
        //            Items = campusesDtos,
        //            HasNextPage = startIndex + pageSize > PAGE_SIZE_LIMIT &&
        //            page < (int)Math.Ceiling((double)totalRows / pageSize),
        //            HasPreviousPage = page > 1
        //        }
        //    };
        //}

        //// Servicio para obtener un campus por ID

        //public async Task<ResponseDto<CampusesDto>> GetOneByIdAsync(string id)
        //{
        //    var campus = await _context.Campuses
        //        .Include(c => c.Careers)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (campus is null)
        //    {
        //        return new ResponseDto<CampusesDto>
        //        {
        //            StatusCode = HttpStatusCode.NOT_FOUND,
        //            Status = false,
        //            Message = "Campus no encontrado.",
        //            Data = null
        //        };
        //    }

        //    return new ResponseDto<CampusesDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Status = true,
        //        Message = "Campus encontrado.",
        //        Data = _mapper.Map<CampusesDto>(campus)
        //    };
        //}

        //// Crear un campus
        //public async Task<ResponseDto<CampusesDto>> CreateAsync(CampusesCreateDto campus)
        //{

        //    if (campus.CareersIds.Any())
        //    {
        //        var validCareers = await _context.UniversityCareers
        //            .Where(c => campus.CareersIds.Contains(c.Id))
        //            .ToListAsync();

        //        if (validCareers.Count != campus.CareersIds.Count)
        //        {
        //            return new ResponseDto<CampusesDto>
        //            {
        //                StatusCode = HttpStatusCode.BAD_REQUEST,
        //                Status = false,
        //                Message = "Algunos IDs de carreras no son válidos.",
        //                Data = null
        //            };
        //        }
        //    }

        //    var campusEntity = _mapper.Map<CampusEntity>(campus);

        //    await _context.Campuses.AddAsync(campusEntity);


        //    return new ResponseDto<CampusesDto>
        //    {
        //        StatusCode = HttpStatusCode.CREATED,
        //        Status = true,
        //        Message = "Campus creado correctamente.",
        //        Data = _mapper.Map<CampusesDto>(campusEntity)
        //    };

        //}

        //// Editar un campus

        //public async Task<ResponseDto<CampusesDto>> EditAsync(CampusesEditDto campus, string id)
        //{
        //    var campusEntity = await _context.Campuses
        //        .Include(c => c.Careers)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (campusEntity is null)
        //    {
        //        return new ResponseDto<CampusesDto>
        //        {
        //            StatusCode = HttpStatusCode.NOT_FOUND,
        //            Status = false,
        //            Message = "Campus no encontrado.",
        //            Data = null
        //        };
        //    }

        //    _mapper.Map(campus, campusEntity);

        //    if (campus.CareersIds.Any())
        //    {
        //        var validCareers = await _context.UniversityCareers
        //            .Where(c => campus.CareersIds.Contains(c.Id))
        //            .ToListAsync();
        //        if (validCareers.Count != campus.CareersIds.Count)
        //        {
        //            return new ResponseDto<CampusesDto>
        //            {
        //                StatusCode = HttpStatusCode.BAD_REQUEST,
        //                Status = false,
        //                Message = "Algunos IDs de carreras no son válidos.",
        //                Data = null
        //            };
        //        }
        //    }

        //    _context.Campuses.Update(campusEntity);
        //    await _context.SaveChangesAsync();
        //    return new ResponseDto<CampusesDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Status = true,
        //        Message = "Campus editado correctamente.",
        //        Data = _mapper.Map<CampusesDto>(campusEntity)
        //    };
        //}

        //// Eliminar un campus

        //public async Task<ResponseDto<CampusesDto>> DeleteAsync(string id)
        //{
        //    var campusEntity = await _context.Campuses
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (campusEntity is null)
        //    {
        //        return new ResponseDto<CampusesDto>
        //        {
        //            StatusCode = HttpStatusCode.NOT_FOUND,
        //            Status = false,
        //            Message = "Campus no encontrado.",
        //            Data = null
        //        };
        //    }

        //    _context.Campuses.Remove(campusEntity);
        //    await _context.SaveChangesAsync();

        //    return new ResponseDto<CampusesDto>
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Status = true,
        //        Message = "Campus eliminado correctamente.",
        //        Data = _mapper.Map<CampusesDto>(campusEntity)
        //    };
        //}
    }
}
