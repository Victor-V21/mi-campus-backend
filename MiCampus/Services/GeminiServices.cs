// using Google.Cloud.AIPlatform.V1;
// using Google.Protobuf.WellKnownTypes;
// using MiCampus.Constants;
// using MiCampus.Dtos.Common;
// using MiCampus.Dtos.Gemini;
// using MiCampus.Services.Interfaces;
// using System.Linq;

// namespace MiCampus.Services
// {
//     public sealed class GeminiServices : IGeminiServices
//     {
//         private readonly PredictionServiceClient _predictionServiceClient;
//         private readonly EndpointName _endpointName;
//         private readonly ILogger<GeminiServices> _logger;

//         public GeminiServices(IConfiguration configuration, ILogger<GeminiServices> logger)
//         {
//             _logger = logger;
//             string projectId = configuration["Gemini:ProjectId"];
//             string location = configuration["Gemini:Location"];
//             string model = "gemini-1.5-flash-001"; // Modelo rápido y eficiente

//             if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(location))
//             {
//                 _logger.LogError("ProjectId o Location de Gemini no están configurados en appsettings.json");
//                 throw new InvalidOperationException("La configuración de Gemini (ProjectId, Location) es inválida.");
//             }

//             _predictionServiceClient = new PredictionServiceClientBuilder
//             {
//                 Endpoint = $"{location}-aiplatform.googleapis.com"
//             }.Build();

//             _endpointName = EndpointName
//                 .FromProjectLocationPublisherModel(projectId, location, "google", model);
//         }

//         public async Task<ResponseDto<GeminiResponseDto>> GenerateContentAsync(GeminiRequestDto dto)
//         {
//             try
//             {
//                 var partsList = new ListValue();
//                 partsList.Values.Add(new Value
//                 {
//                     StructValue = new Struct
//                     {
//                         Fields = { { "text", Value.ForString(dto.Prompt) } }
//                     }
//                 });

//                 // var contentList = new ListValue();
//                 // contentList.Values.Add(new Value
//                 // {
//                 //     StructValue = new Struct
//                 //     {
//                 //         Fields =
//                 // {
//                 //     { "role", Value.ForString("user") },
//                 //     { "parts", new Value { ListValue = partsList } }
//                 // }
//                 //     }
//                 // });

//                 // var request = new PredictRequest
//                 // {
//                 //     EndpointAsEndpointName = _endpointName,
//                 // };

//                 // request.Instances.Add(new Value
//                 // {
//                 //     StructValue = new Struct
//                 //     {
//                 //         Fields =
//                 // {
//                 //     { "contents", new Value { ListValue = contentList } }
//                 // }
//                 //     }
//                 // });

//                 var response = await _predictionServiceClient.PredictAsync(request);
//                 var generatedText = GetTextFromPrediction(response);

//                 return new ResponseDto<GeminiResponseDto>
//                 {
//                     StatusCode = HttpStatusCode.OK,
//                     Status = true,
//                     Message = "Respuesta generada correctamente.",
//                     Data = new GeminiResponseDto { Response = generatedText ?? "No se pudo extraer una respuesta." }
//                 };
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error al generar contenido con Gemini");
//                 return new ResponseDto<GeminiResponseDto>
//                 {
//                     StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
//                     Status = false,
//                     Message = $"Ocurrió un error al comunicarse con el servicio de Gemini: {ex.Message}"
//                 };
//             }
//         }

//         private string GetTextFromPrediction(PredictResponse predictionResponse)
//         {
//             var prediction = predictionResponse.Predictions.FirstOrDefault();
//             if (prediction == null || !prediction.StructValue.Fields.TryGetValue("candidates", out var candidatesValue))
//                 return null;

//             var candidate = candidatesValue.ListValue.Values.FirstOrDefault();
//             if (candidate == null || !candidate.StructValue.Fields.TryGetValue("content", out var contentValue))
//                 return null;

//             var part = contentValue.StructValue.Fields["parts"].ListValue.Values.FirstOrDefault();
//             if (part == null || !part.StructValue.Fields.TryGetValue("text", out var textValue))
//                 return null;

//             return textValue.StringValue;
//         }
//     }
// }