using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/careers")]
    public class CareersController : ControllerBase
    {
        private readonly ICareersServices _careersServices;
        public CareersController(ICareersServices careersServices)
        {
            _careersServices = careersServices;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<CareerActionResponseDto>>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _careersServices.GetEnabledListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<CareerActionResponseDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        // SOLO PARA ADMINISTRADORES
        [HttpGet("admin")]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<CareerDto>>>>> GetList
        (string searchTerm, string isEnabled, int page, int pageSize)
        {
            var response = await _careersServices.GetListAsync(searchTerm, isEnabled, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<CareerDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CareerDto>>> GetByIdAsync(string id)
        {
            var response = await _careersServices.GetByIdAsync(id);

            return StatusCode(response.StatusCode, new ResponseDto<CareerDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<CareerDto>>> CreateAsync([FromBody] CareerCreateDto dto)
        {
            var response = await _careersServices.CreateAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<CareerDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<CareerDto>>> EditAsync(string id, [FromBody] CareerEditDto dto)
        {
            var response = await _careersServices.EditAsync(id, dto);

            return StatusCode(response.StatusCode, new ResponseDto<CareerDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}