using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Auth;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>>
            Login(LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<LoginResponseDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }

    }
}
