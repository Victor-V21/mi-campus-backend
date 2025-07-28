
using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class CareersServices : ICareersServices
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
        // Get list de las carreras habilitadas
        public async Task<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<CareerEntity> careerQuery = _context.Careers.Where(x => x.IsEnabled);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                careerQuery = careerQuery
                    .Where(x => (x.Name + " " + x.Description)
                    .Contains(searchTerm));
            }

            int totalRows = await careerQuery.CountAsync();

            var careersEntity = await careerQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var careersDtos = careersEntity.Adapt<List<CareerActionResponseDto>>();

            return new ResponseDto<PaginationDto<List<CareerActionResponseDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = new PaginationDto<List<CareerActionResponseDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = careersDtos,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                        page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }
        //Get List de todas las carreras (para admin)
        public async Task<ResponseDto<PaginationDto<List<CareerDto>>>> GetListAsync(
            string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<CareerEntity> careerQuery = _context.Careers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                careerQuery = careerQuery
                    .Where(x => (x.Name + " " + x.Description)
                    .Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(isEnabled))
            {
                bool isEnabledBool = bool.Parse(isEnabled);
                careerQuery = careerQuery.Where(x => x.IsEnabled == isEnabledBool);
            }

            int totalRows = await careerQuery.CountAsync();

            var careersEntity = await careerQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var careersDtos = careersEntity.Adapt<List<CareerDto>>();

            return new ResponseDto<PaginationDto<List<CareerDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<CareerDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = careersDtos,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                        page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // Get by Id
        public async Task<ResponseDto<CareerDto>> GetByIdAsync(string id)
        {
            var careerEntity = await _context.Careers
                .Where(x => x.Id == id && x.IsEnabled)
                .FirstOrDefaultAsync();

            if (careerEntity == null)
            {
                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Carrera no encontrada"
                };
            }

            var careerDto = careerEntity.Adapt<CareerDto>();

            return new ResponseDto<CareerDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Carrera encontrada correctamente",
                Data = careerDto
            };
        }

        public async Task<ResponseDto<CareerDto>> CreateAsync(CareerCreateDto dto)
        {
            var careerEntity = dto.Adapt<CareerEntity>();

            _context.Careers.Add(careerEntity);
            await _context.SaveChangesAsync();

            var createdCareerDto = careerEntity.Adapt<CareerDto>();

            return new ResponseDto<CareerDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Carrera creada correctamente",
                Data = createdCareerDto
            };
        }

        public async Task<ResponseDto<CareerDto>> EditAsync(string id, CareerEditDto dto)
        {
            var careerEntity = await _context.Careers.FirstOrDefaultAsync(x => x.Id == id);

            if (careerEntity is null)
            {
                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Carrera no encontrada"
                };
            }
            // Actualiza los datos existentes con los datos de entrada
            // los datos en el dto se añaden al entity
            dto.Adapt(careerEntity);

            _context.Careers.Update(careerEntity);
            await _context.SaveChangesAsync();

            var updatedCareerDto = careerEntity.Adapt<CareerDto>();

            return new ResponseDto<CareerDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Carrera actualizada correctamente",
                Data = updatedCareerDto
            };
        }

    }
}