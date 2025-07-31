
using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class CampusesServices : ICampusesServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public CampusesServices(
            CampusDbContext context,
            IConfiguration configuration
            )
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        // Devuelve la lista de campus con paginación y búsqueda
        public async Task<ResponseDto<PaginationDto<List<CampusActionResponseDto>>>> GetEnabledListAsync(
            string seachTerm = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            // Va encontrar los campus habilitados y aplicar la búsqueda si se proporciona
            IQueryable<CampusEntity> campusQuery = _context.Campuses.Where(x => x.IsEnabled);

            if (!string.IsNullOrEmpty(seachTerm))
            {
                campusQuery = campusQuery
                    .Where(x => (x.Name + " " + x.Description + " " + x.Location)
                    .Contains(seachTerm));
            }

            int totalRows = await campusQuery.CountAsync();

            var campusesEntity = await campusQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var campusesDtos = campusesEntity.Adapt<List<CampusActionResponseDto>>();

            return new ResponseDto<PaginationDto<List<CampusActionResponseDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<CampusActionResponseDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = campusesDtos,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                        page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // Devuelve la lista de campus con paginación y búsqueda, incluyendo los deshabilitados
        public async Task<ResponseDto<PaginationDto<List<CampusDto>>>> GetListAllAsync
        (
            string seachTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            // Va encontrar los campus habilitados y aplicar la búsqueda si se proporciona
            IQueryable<CampusEntity> campusQuery = _context.Campuses;

            if (isEnabled.ToLower() == "true")
            {
                campusQuery = campusQuery.Where(x => x.IsEnabled);
            }
            else if (isEnabled.ToLower() == "false")
            {
                campusQuery = campusQuery.Where(x => !x.IsEnabled);
            }

            if (!string.IsNullOrEmpty(seachTerm))
            {
                campusQuery = campusQuery
                    .Where(x => (x.Name + " " + x.Description + " " + x.Location + " ")
                    .Contains(seachTerm));
            }

            int totalRows = await campusQuery.CountAsync();

            var campusesEntity = await campusQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var campusesDtos = campusesEntity.Adapt<List<CampusDto>>();

            return new ResponseDto<PaginationDto<List<CampusDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<CampusDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = campusesDtos,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // Obtener un campus por ID
        public async Task<ResponseDto<CampusDto>> GetOneByIdAsync(string id)
        {
            var campusEntity = await _context.Campuses.FirstOrDefaultAsync(x => x.Id == id);

            if (campusEntity is null)
            {
                return new ResponseDto<CampusDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            var campusDto = campusEntity.Adapt<CampusDto>();

            return new ResponseDto<CampusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro encontrado correctamente",
                Data = campusDto
            };
        }

        // Crear un nuevo campus
        public async Task<ResponseDto<CampusDto>> CreateAsync(CampusCreateDto dto)
        {
            if (dto == null)
            {
                return new ResponseDto<CampusDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Los datos del campus son inválidos"
                };
            }

            var campusEntity = dto.Adapt<CampusEntity>();

            _context.Campuses.Add(campusEntity);
            await _context.SaveChangesAsync();

            var campusDto = campusEntity.Adapt<CampusDto>();

            return new ResponseDto<CampusDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Campus creado correctamente",
                Data = campusDto
            };
        }

        // Actualizar un campus existente
        public async Task<ResponseDto<CampusDto>> EditAsync(string id, CampusEditDto dto)
        {
            var campusEntity = await _context.Campuses.FirstOrDefaultAsync(x => x.Id == id);

            if (campusEntity is null || dto is null)
            {
                return new ResponseDto<CampusDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado o datos inválidos"
                };
            }

            dto.Adapt(campusEntity);

            _context.Campuses.Update(campusEntity);

            await _context.SaveChangesAsync();

            var campusDto = campusEntity.Adapt<CampusDto>();

            return new ResponseDto<CampusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Campus actualizado correctamente",
                Data = campusDto
            };
        }

        // Agregar carreras al campus
        public async Task<ResponseDto<CampusDto>> AddCareerAsync(string campusId, string careerId)
        {
            var campusEntity = await _context.Campuses.FirstOrDefaultAsync(x => x.Id == campusId);
            var careerEntity = await _context.Careers.FirstOrDefaultAsync(x => x.Id == careerId);

            if (campusEntity is null || careerEntity is null)
            {
                return new ResponseDto<CampusDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Campus no encontrado"
                };
            }

            var invalidCareer = await _context.CampusCareers
                .AnyAsync(c => c.CampusId == campusId && c.CareerId == careerId);

            if (invalidCareer)
            {
                return new ResponseDto<CampusDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "La carrera ya está asociada a este campus"
                };
            }

            var campusCareerEntity = new CampusCareerEntity
            {
                CampusId = campusId,
                CareerId = careerId
            };

            _context.CampusCareers.Add(campusCareerEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<CampusDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Carrera asociada correctamente"
            };
        }

        // Ver las carreras asociadas al campus

        /*
        
            FALTAN LISTAR LAS CARRERAS POR CAMPUS
            
        */
        public async Task<ResponseDto<CampuseCareerDto>> GetCareersByCampusAsync(string campusId, string careerId)
        {
            var campusCareerEntity = await _context.CampusCareers
                .Where(x => x.CampusId == campusId && x.CareerId == careerId)
                .Include(c => c.Career)
                .ToListAsync();

            if (campusCareerEntity is null)
            {
                return new ResponseDto<CampuseCareerDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Campus no encontrado"
                };
            }

            var responseDto = campusCareerEntity.Adapt<CampuseCareerDto>();

            return new ResponseDto<CampuseCareerDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Mensajes encontrados correctamente",
                Data = responseDto
            };
        }

        public async Task<ResponseDto<CampuseCareerDto>> RemoveCareerAsync(string campusId, string careerId)
        {
            var campusCareerEntity = await _context.CampusCareers
                .FirstOrDefaultAsync(c => c.CampusId == campusId && c.CareerId == careerId);

            var careerEntity = await _context.Careers.FirstOrDefaultAsync(x => x.Id == careerId);

            if (campusCareerEntity is null)
            {
                return new ResponseDto<CampuseCareerDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "La carrera no está asociada a este campus"
                };
            }

            _context.CampusCareers.Remove(campusCareerEntity);
            await _context.SaveChangesAsync();

            var responseDto = campusCareerEntity.Adapt<CampuseCareerDto>();

            responseDto.Careers = [careerEntity.Adapt<CareerActionResponseDto>()];

            return new ResponseDto<CampuseCareerDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Carrera desasociada correctamente",
                Data = responseDto
            };
        }

    }
}