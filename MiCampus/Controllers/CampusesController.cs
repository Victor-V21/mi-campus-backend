using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/campuses")]
    public class CampusesController : ControllerBase
    {
        private readonly ICampusesServices _campusesServices;
        public CampusesController(
            ICampusesServices campusesServices
        )
        {
            _campusesServices = campusesServices;
        }

        [HttpGet]

        public async Task<ActionResult<ResponseDto<PaginationDto<List<CampusActionResponseDto>>>>>
            EnabledList(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var response = await _campusesServices.GetEnabledListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<CampusActionResponseDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet]
        [Route("admin")]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<CampusDto>>>>>
            ListAll(string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 10)
        {
            var response = await _campusesServices.GetListAllAsync(searchTerm, isEnabled, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<CampusDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CampusDto>>> GetOneById(string id)
        {
            var response = await _campusesServices.GetOneByIdAsync(id);
            return StatusCode(response.StatusCode, new ResponseDto<CampusDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<CampusDto>>> CrateeAsync(CampusCreateDto dto)
        {
            var response = await _campusesServices.CreateAsync(dto);
            return StatusCode(response.StatusCode, new ResponseDto<CampusDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<CampusDto>>> EditAsync(string id, CampusEditDto dto)
        {
            var response = await _campusesServices.EditAsync(id, dto);
            return StatusCode(response.StatusCode, new ResponseDto<CampusDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        // ENDPOINTS EXTRAS CON CONECCIONES A OTRAS TABLAS
        [HttpPost("careers")]
        public async Task<ActionResult<ResponseDto<CampusDto>>> AddCareerAsync(string campusId, string careerId)
        {
            var response = await _campusesServices.AddCareerAsync(campusId, careerId);

            return StatusCode(response.StatusCode, new ResponseDto<CampusDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("{campusId}/careers/{careerId}")]
        public async Task<ActionResult<ResponseDto<CampuseCareerDto>>> GetCareersByCampus(string campusId, string careerId)
        {
            var response = await _campusesServices.GetCareersByCampusAsync(campusId, careerId);

            return StatusCode(response.StatusCode, new ResponseDto<CampuseCareerDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpDelete("{campusId}/careers/{careerId}")]
        public async Task<ActionResult<ResponseDto<CampuseCareerDto>>> RemoveCareer(string campusId, string careerId)
        {
            var response = await _campusesServices.RemoveCareerAsync(campusId, careerId);

            return StatusCode(response.StatusCode, new ResponseDto<CampuseCareerDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }


    }
}