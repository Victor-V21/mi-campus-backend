using MiCampus.Dtos.Common;
using MiCampus.Dtos.Feedback;
using MiCampus.Dtos.Publication;
using MiCampus.Dtos.PublicationTypes;
using MiCampus.Services;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/publication")]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationServices _publicationServices;
        public PublicationController(
          IPublicationServices publicationServices
        )
        {
            _publicationServices = publicationServices;
        }


        //  listar publicaciones
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<PublicationDto>>>>> GetEnabledListAsync(
          string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _publicationServices.GetEnabledListAsync(searchTerm, page, pageSize);
            return StatusCode((int)response.StatusCode, new ResponseDto<PaginationDto<List<PublicationDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        // crear publicación
        [HttpPost]
        public async Task<ActionResult<ResponseDto<PublicationDto>>> CreateAsync([FromBody] PublicationCreateDto dto)
        {
            var response = await _publicationServices.CreateAsync(dto);

            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        // detalle de publicación solo con el id 
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<PublicationDto>>> GetByIdAsync(string id)
        {
            var response = await _publicationServices.GetByIdAsync(id);

            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
        //editamos la publicaccion

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<PublicationDto>>> EditAsync(string id, [FromBody] PublicationEditDto dto)
        {
            var response = await _publicationServices.EditAsync(id, dto);

            return StatusCode((int)response.StatusCode, new ResponseDto<PublicationDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        //eliminamos la publicacion 
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteAsync(string id)
        {
            var response = await _publicationServices.DeleteAsync(id);

            return StatusCode((int)response.StatusCode, new ResponseDto<object>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

        //agremamos imagenes a la publicaciones

        [HttpPost("{publicationId}/images")]
        public async Task<ActionResult<ResponseDto<object>>> UploadImageAsync(
         [FromRoute] string publicationId, [FromForm] IFormFile file)
        {
            var response = await _publicationServices.UploadImageAsync(publicationId, file);
            return StatusCode((int)response.StatusCode, response);
        }

        //eliminar imagen

        [HttpDelete("{publicationId}/images/{imageId}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteImageAsync(string publicationId, string imageId)
        {
            var response = await _publicationServices.DeleteImageAsync(publicationId, imageId);
            return StatusCode((int)response.StatusCode, response);
        }

        //creamos el cometario 
        [HttpPost("{id}/feedback")]
        public async Task<ActionResult<ResponseDto<FeedbackDto>>> CreateFeedbackAsync(string id, [FromBody] FeekbackCreateDto dto)
        {
            var response = await _publicationServices.CreateFeedbackAsync(id, dto);
            return StatusCode((int)response.StatusCode, response);
        }

        //buscamos el comentario
        [HttpGet("{id}/feedback")]
        public async Task<ActionResult<ResponseDto<List<FeedbackDto>>>> GetFeedbacksAsync(string id)
        {
            var response = await _publicationServices.GetFeedbacksAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        //editamos el comentario 

        [HttpPut("feedback/{feedbackId}")]
        public async Task<ActionResult<ResponseDto<FeedbackDto>>> EditFeedbackAsync(string feedbackId, [FromBody] FeedbackEditDto dto)
        {
            var response = await _publicationServices.EditFeedbackAsync(feedbackId, dto);
            return StatusCode((int)response.StatusCode, response);
        }

        // eliminamos el comentario 

        [HttpDelete("feedback/{feedbackId}")]
        public async Task<ActionResult<ResponseDto<object>>> DeleteFeedbackAsync(string feedbackId, [FromQuery] string userId)
        {
            var response = await _publicationServices.DeleteFeedbackAsync(feedbackId, userId);
            return StatusCode((int)response.StatusCode, response);
        }
        // aun detalles con este y correciones en el siguiente commit 


    }
}
