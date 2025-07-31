using MiCampus.Dtos.Common;
using MiCampus.Dtos.Subjects;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsServices _subjectsServices;
        public SubjectsController(ISubjectsServices subjectsServices)
        {
            _subjectsServices = subjectsServices;
        }
        // Obtener lista de materias habilitadas
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _subjectsServices.GetEnabledListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        // Obtener lista de materias (admin)
        [HttpGet]
        [Route("admin")]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<SubjectActionResponseDto>>>>> GetAllAsync(
            string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0)
        {
            var response = await _subjectsServices.GetAllAsync(searchTerm, isEnabled, page, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        // Obtener materia por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<SubjectDto>>> GetByIdAsync(string id)
        {
            var response = await _subjectsServices.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Crear nueva materia
        [HttpPost]
        public async Task<ActionResult<ResponseDto<SubjectDto>>> CreateAsync([FromBody] SubjectCreateDto dto)
        {
            var response = await _subjectsServices.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // Actualizar materia
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<SubjectDto>>> UpdateAsync(string id, [FromBody] SubjectEditDto dto)
        {
            var response = await _subjectsServices.UpdateAsync(id, dto);

            return StatusCode(response.StatusCode, response);
        }
    }
}