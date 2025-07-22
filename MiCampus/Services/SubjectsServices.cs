using AutoMapper;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Roles;
using MiCampus.Dtos.Subjects;
using MiCampus.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MiCampus.Services
{
    public class SubjectsServices : ISubjectsServices
    {
        private readonly CampusDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public SubjectsServices(
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

        // Paginacion 

    //    public async Task<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>> GetListAsync(
    //        string searchTerm = "", int page = 1, int pageSize = 10)
    //    {
    //        pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;
    //        int startIndex = (page - 1) * pageSize;

    //        IQueryable<SubjectEntity> subjectsQuery = _context.Subjetcs.Include(s => s.Requisites);

    //        if (!string.IsNullOrEmpty(searchTerm))
    //        {
    //            subjectsQuery = subjectsQuery
    //                .Where(s => (s.Name + " " + s.Code)
    //                .Contains(searchTerm));
    //        }

    //        int totalRows = await subjectsQuery.CountAsync();

    //        var subjects = await subjectsQuery
    //            .OrderBy(s => s.Name)
    //            .Skip(startIndex)
    //            .Take(pageSize)
    //            .ToListAsync();

    //        var subjectsDtos = _mapper.Map<List<SubjectActionResponseDto>>(subjects);

    //        return new ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Status = true,
    //            Message = "Registros encontrados correctamente",
    //            Data = new PaginationDto<List<SubjectActionResponseDto>>
    //            {
    //                CurrentPage = page,
    //                PageSize = pageSize,
    //                TotalItems = totalRows,
    //                TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
    //                Items = subjectsDtos,
    //                HasNextPage = startIndex + pageSize > PAGE_SIZE_LIMIT &&
    //                page < (int)Math.Ceiling((double)totalRows / pageSize),
    //                HasPreviousPage = page > 1
    //            }
    //        };
    //    }

    //    public async Task<ResponseDto<SubjectDto>> GetOneByIdAsync(string id)
    //    {
    //        var subject = await _context.Subjetcs
    //            .Include(s => s.Requisites)
    //            .Include(s => s.Career)
    //            .FirstOrDefaultAsync(x => x.Id == id);

    //        if (subject == null)
    //        {
    //            return new ResponseDto<SubjectDto>
    //            {
    //                StatusCode = HttpStatusCode.NOT_FOUND,
    //                Status = false,
    //                Message = "Materia no encontrada."
    //            };
    //        }

    //        return new ResponseDto<SubjectDto>
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Status = true,
    //            Message = "Materia obtenida correctamente.",
    //            Data = _mapper.Map<SubjectDto>(subject)
    //        };
    //    }
    //    //crear para buscar por codigo y nombre

    //    // cear una materia
    //    public async Task<ResponseDto<SubjectDto>> CreateAsync(SubjectCreateDto dto)
    //    {
    //        var validIdsRequisites = await _context.Subjetcs
    //            .Where(s => dto.IdsRequisites.Contains(s.Id))
    //            .ToListAsync();

    //        if (validIdsRequisites.Count != dto.IdsRequisites.Count)
    //        {
    //            return new ResponseDto<SubjectDto>
    //            {
    //                StatusCode = HttpStatusCode.BAD_REQUEST,
    //                Status = false,
    //                Message = "Algunos requisitos no son válidos"
    //            };
    //        }

    //        var subject = _mapper.Map<SubjectEntity>(dto);
    //        subject.Requisites = validIdsRequisites;

    //        _context.Subjetcs.Add(subject);
    //        await _context.SaveChangesAsync();

    //        return new ResponseDto<SubjectDto>
    //        {
    //            StatusCode = HttpStatusCode.CREATED,
    //            Status = true,
    //            Message = "Registro creado correctamente",
    //            Data = _mapper.Map<SubjectDto>(subject)
    //        };
    //    }

    //    // actualizar una materia
    //    public async Task<ResponseDto<SubjectDto>> UpdateAsync(string id, SubjectEditDto dto)
    //    {
    //        var subject = await _context.Subjetcs
    //            .Include(s => s.Requisites)
    //            .FirstOrDefaultAsync(x => x.Id == id);

    //        if (subject == null)
    //        {
    //            return new ResponseDto<SubjectDto>
    //            {
    //                StatusCode = HttpStatusCode.NOT_FOUND,
    //                Status = false,
    //                Message = "Materia no encontrada"
    //            };
    //        }

    //        var validIdsRequisites = await _context.Subjetcs
    //            .Where(s => dto.IdsRequisites.Contains(s.Id))
    //            .ToListAsync();

    //        if (validIdsRequisites.Count != dto.IdsRequisites.Count)
    //        {
    //            return new ResponseDto<SubjectDto>
    //            {
    //                StatusCode = HttpStatusCode.BAD_REQUEST,
    //                Status = false,
    //                Message = "Algunos requisitos no son válidos"
    //            };
    //        }

    //        subject = _mapper.Map<SubjectEntity>(dto);

    //        _context.Subjetcs.Update(subject);

    //        await _context.SaveChangesAsync();

    //        return new ResponseDto<SubjectDto>
    //        {
    //            StatusCode = HttpStatusCode.OK,
    //            Status = true,
    //            Message = "Registro actualizado correctamente",
    //            Data = _mapper.Map<SubjectDto>(subject)
    //        };
    //    }
    }
}
