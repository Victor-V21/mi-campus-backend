using System.ComponentModel.DataAnnotations;

namespace MiCampus.Dtos.Gemini
{
    public class GeminiRequestDto
    {
        [Required]
        public string Prompt { get; set; }
    }
}
