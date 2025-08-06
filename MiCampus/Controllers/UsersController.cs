using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Users;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _usersService;
        private readonly UserManager<UserEntity> _userManager;

        public UsersController(IUsersServices usersService, UserManager<UserEntity> userManager)
        {
            _usersService = usersService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto<UserActionResponseDto>>> Register([FromBody] UserCreateDto dto)
        {
            var response = await _usersService.RegisterAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<UserActionResponseDto>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Parámetros inválidos.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Token de confirmación inválido o expirado.");

            return Ok("Correo confirmado correctamente. Ya puedes iniciar sesión.");
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<UserActionResponseDto>>> Create([FromBody] UserCreateDto dto)
        {
            var response = await _usersService.CreateAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<UserActionResponseDto>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<UserDto>>>>> GetPaginationList
            (string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var response = await _usersService.GetListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, new ResponseDto<PaginationDto<List<UserDto>>>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<PaginationDto<UserDto>>>> GetOne(string id)
        {
            var response = await _usersService.GetOneByIdAsync(id);

            return StatusCode(response.StatusCode, new ResponseDto<UserDto>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<UserActionResponseDto>>> EditById([FromBody] UserEditDto dto, string id)
        {
            var response = await _usersService.EditAsync(dto, id);

            return StatusCode(response.StatusCode, new ResponseDto<UserActionResponseDto>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<UserActionResponseDto>>> DeleteById(string id)
        {
            var response = await _usersService.DeleteAsync(id);

            return StatusCode(response.StatusCode, new ResponseDto<UserActionResponseDto>
            {
                StatusCode = response.StatusCode,
                Status = response.Status,
                Message = response.Message,
                Data = response.Data,
            });
        }
    }
}
