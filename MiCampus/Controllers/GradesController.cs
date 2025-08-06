using MiCampus.Dtos.Common;
using MiCampus.Dtos.Grades;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/grades")]
    public class GradesController : ControllerBase
    {
        private readonly IGradesServices _gradesServices;
        public GradesController(IGradesServices gradesServices)
        {
            _gradesServices = gradesServices;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<GradeDto>>>>> GetList(
            string seachTerm = "", string isEnabled = "", int page = 1, int pageSize = 10
        )
        {
            var response = await _gradesServices.GetListAsync(seachTerm, isEnabled, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<GradeDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<GradeDto>>> CreateAsync(GradeCreateDto dto)
        {
            var response = await _gradesServices.CreateAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<GradeDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<GradeDto>>> UpdateAsync(string id, GradeCreateDto dto)
        {
            var response = await _gradesServices.UpdateAsync(id, dto);

            return StatusCode(response.StatusCode, new ResponseDto<GradeDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}