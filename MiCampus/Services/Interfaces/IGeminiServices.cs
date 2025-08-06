using MiCampus.Dtos.Common;
using MiCampus.Dtos.Gemini;

namespace MiCampus.Services.Interfaces
{
    public interface IGeminiServices
    {
        Task<ResponseDto<GeminiResponseDto>> GenerateContentAsync(GeminiRequestDto dto);
    }
}