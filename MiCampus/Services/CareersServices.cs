using AutoMapper;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Roles;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class CareersServices : ICareersServices
    {
        private readonly CampusDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        public CareersServices(
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

        
        // servicio get list careers
        public async  Task<ResponseDto<PaginationDto<List<CareerActionResponse>>>> GetListAsync
            (int page = 1, int pageSize = 10, string searchTerm = "")
        {
            page = page > 0 ? page: 1;
            int startIndex = (page - 1) * pageSize;

            IQueryable<UniversityCareerEntity> careerQuery = _context.UniversityCareers;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                careerQuery = careerQuery
                    .Where(x => (x.Name + " " + x.Description)
                    .Contains(searchTerm));
            }

            int totalRows = await careerQuery.CountAsync();

            var careers = careerQuery
                .OrderBy(r => r.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToList();


            return new ResponseDto<PaginationDto<List<CareerActionResponse>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<CareerActionResponse>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = _mapper.Map<List<CareerActionResponse>>(careers),
                    HasNextPage = startIndex + pageSize > PAGE_SIZE_LIMIT &&
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // servicio get one by id
        public async Task<ResponseDto<CareerDto>> GetOneById(string id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var career = await _context.UniversityCareers
                    .Where(x => x.Id == id)
                    .Include(s => s.Subjects)
                    .FirstOrDefaultAsync();

                if (career == null)
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Registro no encontrado",
                        Data = null
                    };
                }

                await transaction.CommitAsync();

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Carrera encontrada correctamente.",
                    Data = _mapper.Map<CareerDto>(career)
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                return new ResponseDto<CareerDto>
                {
                    StatusCode = (int)HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor",
                    Data = null
                };
            }
        }

        // servicio create career
        public async Task<ResponseDto<CareerDto>> CreateAsync(CareerCreateDto dto)
        {
            var validIdsSubjects = await _context.Subjetcs
                .Where(s => dto.IdsSubjects.Contains(s.Id))
                .ToListAsync();

            if (validIdsSubjects.Count != dto.IdsSubjects.Count)
            {
                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Algunos IDs de materias no son válidos.",
                    Data = null
                };
            }

            var career = _mapper.Map<UniversityCareerEntity>(dto);

            await _context.UniversityCareers.AddAsync(career);

            return new ResponseDto<CareerDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Carrera creada correctamente.",
                Data = _mapper.Map<CareerDto>(career)
            };
        }

        // servicio de Editar una carrera
        public async Task<ResponseDto<CareerDto>> EditAsync(CareerEditDto dto, string id)
        {
            var career = await _context.UniversityCareers
                .Include(s => s.Subjects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (career == null)
            {return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado",
                    Data = null
                };
            }

            var validSubjets = career.Subjects
                .Where(s => dto.IdsSubjects.Contains(s.Id))
                .ToList();

            if (validSubjets.Count != dto.IdsSubjects.Count)
            {
                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Algunos IDs de materias no son válidos.",
                    Data = null
                };
            }

           using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                career = _mapper.Map<UniversityCareerEntity>(dto);
                career.Subjects = validSubjets;

                _context.UniversityCareers.Update(career);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Registro editado correctamente",
                    Data = _mapper.Map<CareerDto>(career)
                };
            }

            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ResponseDto<CareerDto>
                {
                    StatusCode = (int)HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor",
                    Data = null
                };
            }

        }

        // servicio de eliminar una carrera

        public async Task<ResponseDto<CareerActionResponse>> DeleteAsync(string id)
        {
            var career = await _context.UniversityCareers.FirstOrDefaultAsync(x => x.Id == id);

            if(career is null)
            {
                return new ResponseDto<CareerActionResponse>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            _context.UniversityCareers.Remove(career);

            await _context.SaveChangesAsync();

            return new ResponseDto<CareerActionResponse>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro eliminado correctamente",
                Data = _mapper.Map<CareerActionResponse>(career)
            };
        }
    }
}
