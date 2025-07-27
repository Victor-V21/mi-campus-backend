using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Roles;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        //En listar
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<RoleDto>>>>> GetListPagination(
            string searchTerm = "", int page = 1, int pageSize = 10
            )
        {
            var response = await _rolesService.GetListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<RoleDto>>>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
        //Buscar por id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<RoleDto>>> GetOneById(string id)
        {

            var response = await _rolesService.GetOneById(id);
            return StatusCode(response.StatusCode,
                new ResponseDto<RoleDto>
                {
                    Status = response.Status,
                    Message = response.Message,
                    Data = response.Data
                });
        }

        //Crear nuevo

        [HttpPost]
        public async Task<ActionResult<ResponseDto<RoleActionResponseDto>>>
            CreateAsync(
            RoleCreateDto dto
        )
        {
            var response = await _rolesService.CreateAsync(dto);
            return StatusCode(response.StatusCode,
                new ResponseDto<RoleActionResponseDto>
                {
                    Status = response.Status,
                    Message = response.Message,
                    Data = response.Data
                });
        }

        //Editar uno

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<RoleActionResponseDto>>> Edit(
            [FromBody] RoleEditDto dto, string id)
        {

            var response = await _rolesService.EditAsync(dto, id);
            return StatusCode(response.StatusCode,
                new ResponseDto<RoleActionResponseDto>
                {
                    Status = response.Status,
                    Message = response.Message,
                    Data = response.Data
                });

        }

        //Eliminar
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<RoleActionResponseDto>>> Delete(string id)
        {

            var response = await _rolesService.DeleteAsync(id);
            return StatusCode(response.StatusCode,
                new ResponseDto<RoleActionResponseDto>
                {
                    Status = response.Status,
                    Message = response.Message,
                    Data = response.Data
                });
        }
    }
}
