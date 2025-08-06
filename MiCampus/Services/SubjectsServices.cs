using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Subjects;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class SubjectsServices : ISubjectsServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        public SubjectsServices(
            CampusDbContext context,
            IConfiguration configuration
        )
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        // Obtener lista de materias habilitadas
        public async Task<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0
        )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<SubjectEntity> subjectsQuery = _context.Subjects.Where(x => x.IsEnabled);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                subjectsQuery = subjectsQuery
                    .Where(x => (x.Name + " " + x.Code + " " + x.Description)
                    .Contains(searchTerm));
            }

            int totalRows = await subjectsQuery.CountAsync();

            List<SubjectEntity> subjects = await subjectsQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var subjectsDtos = subjects.Adapt<List<SubjectActionResponseDto>>();

            return new ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Lista de materias obtenida correctamente",
                Data = new PaginationDto<List<SubjectActionResponseDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = subjectsDtos,
                    HasNextPage = startIndex + pageSize > PAGE_SIZE_LIMIT &&
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                },
            };
        }

        // Obtener tota la lista de materias (incluye las deshabilitadas)
        public async Task<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>> GetAllAsync
        (string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<SubjectEntity> subjectsQuery = _context.Subjects;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                subjectsQuery = subjectsQuery
                    .Where(x => (x.Name + " " + x.Code + " " + x.Description)
                    .Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(isEnabled))
            {
                bool isEnabledBool = bool.Parse(isEnabled);
                subjectsQuery = subjectsQuery.Where(x => x.IsEnabled == isEnabledBool);
            }

            int totalRows = await subjectsQuery.CountAsync();

            List<SubjectEntity> subjects = await subjectsQuery
                .OrderBy(x => x.Name)
                .Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();

            var subjectsDtos = subjects.Adapt<List<SubjectActionResponseDto>>();

            return new ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Lista de materias obtenida correctamente",
                Data = new PaginationDto<List<SubjectActionResponseDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = subjectsDtos,
                    HasNextPage = startIndex + pageSize > PAGE_SIZE_LIMIT &&
                    page < (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1
                },
            };
        }

        //Get One subject by id
        public async Task<ResponseDto<SubjectDto>> GetByIdAsync(string id)
        {
            var subjectEntity = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == id && x.IsEnabled);

            if (subjectEntity is null)
            {
                return new ResponseDto<SubjectDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Materia no encontrada"
                };
            }

            var subjectDto = subjectEntity.Adapt<SubjectDto>();

            return new ResponseDto<SubjectDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Materia obtenida correctamente",
                Data = subjectDto
            };
        }

        // Create a new subject
        public async Task<ResponseDto<SubjectDto>> CreateAsync(SubjectCreateDto dto)
        {

            var subjectEntity = dto.Adapt<SubjectEntity>();

            _context.Subjects.Add(subjectEntity);
            await _context.SaveChangesAsync();

            var createdSubjectDto = subjectEntity.Adapt<SubjectDto>();

            return new ResponseDto<SubjectDto>
            {
                StatusCode = HttpStatusCode.CREATED,
                Status = true,
                Message = "Materia creada correctamente",
                Data = createdSubjectDto
            };
        }

        // Update an existing subject
        public async Task<ResponseDto<SubjectDto>> UpdateAsync(string id, SubjectEditDto dto)
        {
            var subjectEntity = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == id);

            if (subjectEntity is null)
            {
                return new ResponseDto<SubjectDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Status = false,
                    Message = "Materia no encontrada"
                };
            }

            dto.Adapt(subjectEntity);

            _context.Subjects.Update(subjectEntity);
            await _context.SaveChangesAsync();

            var updatedSubjectDto = subjectEntity.Adapt<SubjectDto>();

            return new ResponseDto<SubjectDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Materia actualizada correctamente",
                Data = updatedSubjectDto
            };
        }
    }
}