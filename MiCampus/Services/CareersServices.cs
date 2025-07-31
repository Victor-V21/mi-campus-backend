
using Mapster;
using MiCampus.Constants;
using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Subjects;
using MiCampus.Services.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

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
        public async Task<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>> GetListAsync(
            string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0
        )
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<CareerEntity> careerQuery = _context.Careers;

            if (!string.IsNullOrEmpty(isEnabled))
            {
                if (isEnabled.ToLower() == "true")
                {
                    careerQuery = careerQuery.Where(x => x.IsEnabled);
                }
                else if (isEnabled.ToLower() == "false")
                {
                    careerQuery = careerQuery.Where(x => !x.IsEnabled);
                }
            }

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

        // Get by Id
        public async Task<ResponseDto<CareerDto>> GetByIdAsync(string id)
        {
            var careerEntity = await _context.Careers
                .Where(x => x.Id == id && x.IsEnabled)
                .Include(c => c.CareerSubjects)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.CareerSubjects)
                    .ThenInclude(cs => cs.Requisites)
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

            // Mapear materias y requisitos
            careerDto.Subjects = careerEntity.CareerSubjects.Select(cs =>
            {
                var addSubjectDto = new AddSubjectDto
                {
                    SubjectId = cs.Subject.Id,
                    RequisiteId = cs.Requisites.Select(r => r.RequisiteSubjectId).ToList()
                };

                return addSubjectDto;
            }).ToList();

            return new ResponseDto<CareerDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Carrera encontrada correctamente",
                Data = careerDto
            };
        }

        public async Task<ResponseDto<CareerDto>> GetAllByIdAsync(string id)
        {
            var careerEntity = await _context.Careers
                .Where(x => x.Id == id)
                .Include(c => c.CareerSubjects)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.CareerSubjects)
                    .ThenInclude(cs => cs.Requisites)
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

            // Mapear materias y requisitos
            careerDto.Subjects = careerEntity.CareerSubjects.Select(cs =>
            {
                var addSubjectDto = new AddSubjectDto
                {
                    SubjectId = cs.Subject.Id,
                    RequisiteId = cs.Requisites.Select(r => r.RequisiteSubjectId).ToList()
                };

                return addSubjectDto;
            }).ToList();

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
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validar que la carrera no exista por nombre
                var existsCareer = await _context.Careers
                    .AnyAsync(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower());

                if (existsCareer)
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Ya existe una carrera con ese nombre."
                    };
                }

                // Validar que subjects no venga vacío
                if (dto.Subjects == null || !dto.Subjects.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Debe agregar al menos una materia a la carrera."
                    };
                }

                // Validar materias duplicadas dentro del request
                var duplicatedSubjects = dto.Subjects
                    .GroupBy(s => s.SubjectId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicatedSubjects.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Existen materias repetidas en la solicitud: {string.Join(", ", duplicatedSubjects)}"
                    };
                }

                // Validar que ninguna materia tenga como requisito a sí misma
                var selfRequisites = dto.Subjects
                    .Where(s => s.RequisiteId != null && s.RequisiteId.Contains(s.SubjectId))
                    .Select(s => s.SubjectId)
                    .ToList();

                if (selfRequisites.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Las siguientes materias tienen como requisito a sí mismas: {string.Join(", ", selfRequisites)}"
                    };
                }

                // Validar que no existan requisitos duplicados en cada materia
                var subjectsWithDuplicateReqs = dto.Subjects
                    .Where(s => s.RequisiteId != null && s.RequisiteId.GroupBy(r => r).Any(g => g.Count() > 1))
                    .Select(s => s.SubjectId)
                    .ToList();

                if (subjectsWithDuplicateReqs.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Algunas materias tienen requisitos duplicados: {string.Join(", ", subjectsWithDuplicateReqs)}"
                    };
                }


                // Validar materias y requisitos existentes
                var currentSubjectsIds = await _context.Subjects
                    .Where(x => x.IsEnabled)
                    .Select(x => x.Id)
                    .ToListAsync();

                var subjectsIds = dto.Subjects
                    .Select(x => x.SubjectId)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                var invalidSubjects = subjectsIds.Except(currentSubjectsIds).ToList();
                if (invalidSubjects.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Algunas materias no existen: {string.Join(", ", invalidSubjects)}"
                    };
                }

                var requisitesIds = dto.Subjects
                    .SelectMany(x => x.RequisiteId)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                var invalidRequisites = requisitesIds.Except(currentSubjectsIds).ToList();
                if (invalidRequisites.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = $"Algunos requisitos no existen en la carrera: {string.Join(", ", invalidRequisites)}"
                    };
                }

                // Crear la carrera
                var careerEntity = dto.Adapt<CareerEntity>();
                _context.Careers.Add(careerEntity);
                await _context.SaveChangesAsync(); // Para obtener el Id de la carrera

                // Crear las materias asociadas
                foreach (var subject in dto.Subjects)
                {
                    var careerSubject = new CareerSubjectEntity
                    {
                        CareerId = careerEntity.Id,
                        SubjectId = subject.SubjectId
                    };

                    _context.CareerSubjects.Add(careerSubject);
                    await _context.SaveChangesAsync(); // Para obtener el Id del CareerSubject

                    // Crear requisitos de la materia
                    foreach (var requisite in subject.RequisiteId)
                    {
                        var careerSubjectRequisite = new CareerSubjectRequisiteEntity
                        {
                            CareerSubjectId = careerSubject.Id,
                            RequisiteSubjectId = requisite
                        };

                        _context.CareerSubjectsRequisites.Add(careerSubjectRequisite);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var responseDto = careerEntity.Adapt<CareerDto>();
                responseDto.Subjects = dto.Subjects;

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.CREATED,
                    Status = true,
                    Message = "Carrera creada correctamente",
                    Data = responseDto
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor",
                    Data = null
                };
            }
        }

        /*
            Falta moddificar el servicio para añadir campus y carrera 
        */
        public async Task<ResponseDto<CareerDto>> EditAsync(string id, CareerEditDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var careerEntity = await _context.Careers
                    .Include(c => c.CareerSubjects)
                        .ThenInclude(cs => cs.Requisites)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (careerEntity is null)
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.NOT_FOUND,
                        Status = false,
                        Message = "Carrera no encontrada"
                    };
                }

                // Validar nombre duplicado
                var existsCareer = await _context.Careers
                    .AnyAsync(c => c.Id != id && c.Name.Trim().ToLower() == dto.Name.Trim().ToLower());

                if (existsCareer)
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Ya existe una carrera con ese nombre."
                    };
                }

                // Validar que subjects no venga vacío
                if (dto.Subjects == null || !dto.Subjects.Any())
                {
                    return new ResponseDto<CareerDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Status = false,
                        Message = "Debe agregar al menos una materia a la carrera."
                    };
                }

                // Validaciones de duplicados y requisitos igual que en CreateAsync (puedes reutilizar el código ahí)

                // Actualizar datos básicos
                dto.Adapt(careerEntity);
                _context.Careers.Update(careerEntity);

                // IDs de materias recibidas
                var newSubjectsIds = dto.Subjects.Select(s => s.SubjectId).ToList();

                // Eliminar materias que ya no están en el DTO
                var subjectsToRemove = careerEntity.CareerSubjects
                    .Where(cs => !newSubjectsIds.Contains(cs.SubjectId))
                    .ToList();

                _context.CareerSubjects.RemoveRange(subjectsToRemove);

                // Procesar materias (agregar nuevas o actualizar requisitos)
                foreach (var subjectDto in dto.Subjects)
                {
                    var existingSubject = careerEntity.CareerSubjects
                        .FirstOrDefault(cs => cs.SubjectId == subjectDto.SubjectId);

                    if (existingSubject == null)
                    {
                        // Nueva materia
                        var newCareerSubject = new CareerSubjectEntity
                        {
                            CareerId = careerEntity.Id,
                            SubjectId = subjectDto.SubjectId
                        };
                        _context.CareerSubjects.Add(newCareerSubject);

                        foreach (var req in subjectDto.RequisiteId ?? Enumerable.Empty<string>())
                        {
                            _context.CareerSubjectsRequisites.Add(new CareerSubjectRequisiteEntity
                            {
                                CareerSubject = newCareerSubject,
                                RequisiteSubjectId = req
                            });
                        }
                    }
                    else
                    {
                        // Actualizar requisitos de la materia
                        var currentReqIds = existingSubject.Requisites.Select(r => r.RequisiteSubjectId).ToList();
                        var newReqIds = subjectDto.RequisiteId ?? new List<string>();

                        // Eliminar requisitos que ya no estén
                        var reqsToRemove = existingSubject.Requisites
                            .Where(r => !newReqIds.Contains(r.RequisiteSubjectId))
                            .ToList();
                        _context.CareerSubjectsRequisites.RemoveRange(reqsToRemove);

                        // Agregar nuevos
                        var reqsToAdd = newReqIds.Except(currentReqIds).ToList();
                        foreach (var req in reqsToAdd)
                        {
                            _context.CareerSubjectsRequisites.Add(new CareerSubjectRequisiteEntity
                            {
                                CareerSubjectId = existingSubject.Id,
                                RequisiteSubjectId = req
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var updatedCareerDto = careerEntity.Adapt<CareerDto>();
                updatedCareerDto.Subjects = dto.Subjects;

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Status = true,
                    Message = "Carrera actualizada correctamente",
                    Data = updatedCareerDto
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex);

                return new ResponseDto<CareerDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Status = false,
                    Message = "Error interno en el servidor"
                };
            }
        }
    }
}