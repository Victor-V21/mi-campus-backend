using Microsoft.AspNetCore.Mvc;
using MiCampus.Services.Interfaces;
using MiCampus.Dtos.Gemini;
using MiCampus.Dtos.Common;
using System.Threading.Tasks;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : ControllerBase
    {
        private readonly IGeminiServices _geminiService;

        public GeminiController(IGeminiServices geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<ResponseDto<GeminiResponseDto>>> GenerateContent([FromBody] GeminiRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _geminiService.GenerateContentAsync(requestDto);

            if (!result.Status)
            {
                return StatusCode((int)result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}