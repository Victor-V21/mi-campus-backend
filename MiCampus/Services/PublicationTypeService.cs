using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.PublicationTypes;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class PublicationTypeService : IPublicationTypeServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        public PublicationTypeService(
            CampusDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }
     

        public async Task<ResponseDto<PaginationDto<List<PublicationTypeDto>>>> GetListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;
            int startIndex = (page - 1) * pageSize;

            IQueryable<PublicationTypeEntity> query = _context.PublicationsTypes
                .Where(x => x.IsEnabled);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x =>
                    (x.Name + " " + x.Description).Contains(searchTerm));
            }

            int totalRows = await query.CountAsync();

            var entities = await query
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Adapt<List<PublicationTypeDto>>();

            return new ResponseDto<PaginationDto<List<PublicationTypeDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<PublicationTypeDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = items,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                                  page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // creamos el tipo de publicacion 
        public async Task<ResponseDto<PublicationTypeDto>> CreateAsync(PublicationTypeCreateDto dto)
        {
            
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Los datos del tipo de publicación son inválidos (Name es requerido)."
                };
            }

            var nameNorm = dto.Name.Trim().ToLower();


            var exists = await _context.PublicationsTypes
                .AnyAsync(t => t.Name.Trim().ToLower() == nameNorm);

            if (exists)
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Ya existe un tipo de publicación con ese nombre."
                };
            }


            var entity = dto.Adapt<PublicationTypeEntity>();
            entity.Name = dto.Name.Trim();
            entity.Description = dto.Description?.Trim();
            entity.IsEnabled = true;

            _context.PublicationsTypes.Add(entity);
            await _context.SaveChangesAsync();


            var outDto = entity.Adapt<PublicationTypeDto>();

            return new ResponseDto<PublicationTypeDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Tipo de publicación creado correctamente",
                Data = outDto
            };
        }

        // editamos 
        public async Task<ResponseDto<PublicationTypeDto>> EditAsync(string id, PublicationTypeEditDto dto)
        {
           
            if (dto is null || string.IsNullOrWhiteSpace(id))
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Datos inválidos"
                };
            }

            
            var entity = await _context.PublicationsTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Tipo de publicación no encontrado"
                };
            }

            
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                var nameNorm = dto.Name.Trim().ToLower();
                var duplicated = await _context.PublicationsTypes
                    .AnyAsync(t => t.Id != id && t.Name.Trim().ToLower() == nameNorm);

                if (duplicated)
                {
                    return new ResponseDto<PublicationTypeDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Ya existe un tipo de publicación con ese nombre."
                    };
                }
            }

           
            dto.Adapt(entity);
            entity.Name = entity.Name?.Trim();
            entity.Description = entity.Description?.Trim();

            _context.PublicationsTypes.Update(entity);
            await _context.SaveChangesAsync();

           
            var outDto = entity.Adapt<PublicationTypeDto>();

            return new ResponseDto<PublicationTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Tipo de publicación actualizado correctamente",
                Data = outDto
            };
        }

        // eliminamos 
        public async Task<ResponseDto<PublicationTypeDto>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Id inválido"
                };
            }

            var entity = await _context.PublicationsTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Tipo de publicación no encontrado"
                };
            }

           
            var inUse = await _context.Publications
                .AnyAsync(p => p.TypeId == id && p.IsEnabled);
            if (inUse)
            {
                return new ResponseDto<PublicationTypeDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se puede eliminar: existen publicaciones activas con este tipo."
                };
            }


            entity.IsEnabled = false;
            _context.PublicationsTypes.Update(entity);
            await _context.SaveChangesAsync();

            var dto = entity.Adapt<PublicationTypeDto>();

            return new ResponseDto<PublicationTypeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Tipo de publicación eliminado correctamente",
                Data = dto
            };
        }

    }
}
