using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Grades;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class GradesServices : IGradesServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        public GradesServices(
            CampusDbContext context,
            IConfiguration configuration
            )
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        // Get List de todos los grados academicos 
        public async Task<ResponseDto<PaginationDto<List<GradeDto>>>> GetListAsync(
            string seachTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            // Va encontrar los grados academicos y aplicar la búsqueda si se proporciona
            IQueryable<GradeEntity> gradesQuery = _context.CareerGrades.AsQueryable();

            if (!string.IsNullOrEmpty(seachTerm))
            {
                gradesQuery = gradesQuery
                    .Where(x => (x.Name + " " + x.Description)
                    .Contains(seachTerm));
            }

            if (!string.IsNullOrEmpty(isEnabled))
            {
                bool isEnabledBool = bool.Parse(isEnabled);
                gradesQuery = gradesQuery.Where(x => x.IsEnabled == isEnabledBool);
            }

            int totalRows = await gradesQuery.CountAsync();

            var gradesEntities = await gradesQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var gradesDtos = gradesEntities.Adapt<List<GradeDto>>();

            return new ResponseDto<PaginationDto<List<GradeDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<GradeDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = gradesDtos,
                    HasNextPage = startIndex + pageSize < PAGE_SIZE_LIMIT &&
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                }
            };
        }

        // Crear un nuevo grado academico
        public async Task<ResponseDto<GradeDto>> CreateAsync(GradeCreateDto dto)
        {
            var gradeEntity = dto.Adapt<GradeEntity>();

            _context.CareerGrades.Add(gradeEntity);
            await _context.SaveChangesAsync();

            var createdGradeDto = gradeEntity.Adapt<GradeDto>();

            return new ResponseDto<GradeDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Grado académico creado correctamente",
                Data = createdGradeDto
            };
        }

        // Actualizar un grado academico
        public async Task<ResponseDto<GradeDto>> UpdateAsync(string id, GradeCreateDto dto)
        {
            var gradeEntity = await _context.CareerGrades.FirstOrDefaultAsync(x => x.Id == id);

            if (gradeEntity is null)
            {
                return new ResponseDto<GradeDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }
            // Actualizar las propiedades del grado academico
            // pasa los cambios del dto al entity
            dto.Adapt(gradeEntity);

            _context.CareerGrades.Update(gradeEntity);
            await _context.SaveChangesAsync();

            var updatedGradeDto = gradeEntity.Adapt<GradeDto>();

            return new ResponseDto<GradeDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro actualizado correctamente",
                Data = updatedGradeDto
            };
        }
    }
}