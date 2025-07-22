using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{

    [Route("api/campuses")]
    [ApiController]
    public class CampusesController : ControllerBase
    {
        //private readonly ICampusesServices _campusesServices;
        //public CampusesController(ICampusesServices services)
        //{
        //    services = _campusesServices;
        //}

        //[HttpGet]
        //public async Task<ActionResult<ResponseDto<PaginationDto<List<CampusesDto>>>>> GetListAsync(
        //    [FromQuery] string searchTerm = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        //{
        //    var response = await _campusesServices.GetListAsync(searchTerm, page, pageSize);
        //    return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<CampusesDto>>>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<ResponseDto<CampusesDto>>> GetOneByIdAsync(string id)
        //{
        //    var response = await _campusesServices.GetOneByIdAsync(id);
        //    return StatusCode(response.StatusCode, new ResponseDto<CampusesDto>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });
        //}

        //[HttpPost]

        //public async Task<ActionResult<ResponseDto<CampusesDto>>> CreateAsync(CampusesCreateDto campus)
        //{
        //    var response = await _campusesServices.CreateAsync(campus);
        //    return StatusCode(response.StatusCode, new ResponseDto<CampusesDto>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<ResponseDto<CampusesDto>>> EditAsync(
        //    [FromBody] CampusesEditDto campus,
        //    string id)
        //{
        //    var response = await _campusesServices.EditAsync(campus, id);
        //    return StatusCode(response.StatusCode, new ResponseDto<CampusesDto>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ResponseDto<CampusesDto>>> DeleteAsync(string id)
        //{
        //    var response = await _campusesServices.DeleteAsync(id);
        //    return StatusCode(response.StatusCode, new ResponseDto<CampusesDto>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });

        //}
    }
}
