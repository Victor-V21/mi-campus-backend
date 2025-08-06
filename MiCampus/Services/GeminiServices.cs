using System.Net;
using System.Text;
using System.Text.Json;
using MiCampus.Constants;
using MiCampus.Dtos.Common;
using MiCampus.Dtos.Gemini;
using MiCampus.Helpers;
using MiCampus.Services.Interfaces;

namespace MiCampus.Services
{
    public sealed class GeminiServices : IGeminiServices
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey;

        public GeminiServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            apiKey = Environment
                .GetEnvironmentVariable("GEMINI_API_KEY") ??
                throw new Exception("No se encontró la variable GEMINI_API_KEY en .env");
        }

        public async Task<ResponseDto<GeminiResponseDto>> GenerateContentAsync(GeminiRequestDto dto)
        {
            // 1. Cargar contexto base desde archivo TXT
            var contextPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "gemini-context.txt");
            var contextPrompt = await File.ReadAllTextAsync(contextPath);

            // 2. Buscar fragmentos en múltiples PDFs
            var pdfFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "UniversityReglaments");
            var fragmentosDeVariosPDFs = PdfHelper.BuscarFragmentosEnMultiplesPDFs(pdfFolderPath, dto.Prompt);

            // 3. Construir prompt final
            var fullPrompt = $"{contextPrompt}\n\nInformación relevante de los reglamentos:\n{fragmentosDeVariosPDFs}\n\nPregunta del estudiante:\n{dto.Prompt}";

            // 4. Crear request para Gemin, basado en la documentacion de google ia studio
            //https://aistudio.google.com/apikey
            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = fullPrompt }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage
            (
                HttpMethod.Post,
                "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent"
            );

            request.Headers.Add("X-goog-api-key", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseDto<GeminiResponseDto>
                {
                    Status = false,
                    Message = $"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}"
                };
            }

            var responseContent = await response.Content.ReadAsStreamAsync();
            var jsonDoc = await JsonDocument.ParseAsync(responseContent);

            var text = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            var responseDto = new GeminiResponseDto
            {
                Response = text ?? "No response from Gemini"
            };

            return new ResponseDto<GeminiResponseDto>
            {
                StatusCode = Constants.HttpStatusCode.OK,
                Status = true,
                Message = "Success",
                Data = responseDto
            };
        }
    }
}
