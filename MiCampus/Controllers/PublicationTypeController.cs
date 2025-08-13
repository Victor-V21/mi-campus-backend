using MiCampus.Dtos.Common;
using MiCampus.Dtos.PublicationTypes;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/publication-types")]
    public class PublicationTypesController : ControllerBase
    {
        private readonly IPublicationTypeServices _publicationTypeServices;

        public PublicationTypesController(IPublicationTypeServices publicationTypeServices)
        {
            _publicationTypeServices = publicationTypeServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<PublicationTypeDto>>>>> GetEnabledListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _publicationTypeServices.GetListAsync(searchTerm, page, pageSize);
            return StatusCode((int)response.StatusCode, new ResponseDto<PaginationDto<List<PublicationTypeDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        //// enlistar por el admin 
        //[HttpGet("admin")]
        //public async Task<ActionResult<ResponseDto<PaginationDto<List<PublicationTypeDto>>>>> GetListAsync(
        //    string searchTerm = "", string isEnabled = "", int page = 1, int pageSize = 0)
        //{
        //    var response = await _publicationTypeServices.GetListAsync(searchTerm, isEnabled, page, pageSize);
        //    return StatusCode((int)response.StatusCode, new ResponseDto<PaginationDto<List<PublicationTypeDto>>>
        //    {
        //        Status = response.Status,
        //        Message = response.Message,
        //        Data = response.Data
        //    });
        //}  // aun debo verificar por que no me esta funcionando al 100

        // POST /publication-types
        [HttpPost]
        public async Task<ActionResult<ResponseDto<PublicationTypeDto>>> CreateAsync([FromBody] PublicationTypeCreateDto dto)
        {
            var response = await _publicationTypeServices.CreateAsync(dto);
            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationTypeDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<PublicationTypeDto>>> EditAsync(string id, [FromBody] PublicationTypeEditDto dto)
        {
            var response = await _publicationTypeServices.EditAsync(id, dto);
            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationTypeDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<PublicationTypeDto>>> DeleteAsync(string id)
        {
            var response = await _publicationTypeServices.DeleteAsync(id);
            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationTypeDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}
