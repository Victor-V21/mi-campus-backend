using MiCampus.Dtos.Common;
using MiCampus.Dtos.Security.Auth;

namespace MiCampus.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
}
