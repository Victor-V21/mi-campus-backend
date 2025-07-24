using MiCampus.Dtos.Campuses;
using MiCampus.Dtos.Careers;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    // [Route("api/careers")]
    // [ApiController]
    public class CareersController : ControllerBase
    {
        //private readonly ICareersServices _careersServices;
        //public CareersController( ICareersServices careersServices)
        //{
        //    _careersServices = careersServices;
        //}

        //[HttpGet]
        //public async Task<ActionResult<ResponseDto<List<CareerActionResponse>>>> GetList()
        //{
        //    var result = await _careersServices.GetListAsync();

        //    return StatusCode(result.StatusCode, result);
        //}


        //[HttpGet("{id}")]
        //public async Task<ActionResult<ResponseDto<CareerDto>>> GetOne(string id)
        //{
        //    var result = await _careersServices.GetOneById(id);

        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpPost]
        //public async Task<ActionResult<ResponseDto<CareerDto>>> Create([FromBody] CareerCreateDto dto)
        //{
        //    var result = await _careersServices.CreateAsync(dto);

        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<ResponseDto<CareerDto>>> Edit
        //    ([FromBody] CareerEditDto dto, string id)
        //{
        //    var result = await _careersServices.EditAsync(dto, id);

        //    return StatusCode(result.StatusCode, result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ResponseDto<CareerActionResponse>>> Delete(string id)
        //{
        //    var result = await _careersServices.DeleteAsync(id);

        //    return StatusCode(result.StatusCode, result);
        //}

    }
}
