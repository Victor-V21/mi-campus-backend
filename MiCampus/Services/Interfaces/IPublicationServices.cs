using MiCampus.Dtos.Common;
using MiCampus.Dtos.Feedback;
using MiCampus.Dtos.Publication;

namespace MiCampus.Services.Interfaces
{
    public interface IPublicationServices
    {
        Task<ResponseDto<PublicationDto>> CreateAsync(PublicationCreateDto dto);
        Task<ResponseDto<FeedbackDto>> CreateFeedbackAsync(string publicationId, FeekbackCreateDto dto);
        Task<ResponseDto<object>> DeleteAsync(string id);
        Task<ResponseDto<object>> DeleteFeedbackAsync(string feedbackId, string userId);
        Task<ResponseDto<object>> DeleteImageAsync(string publicationId, string imageId);
        Task<ResponseDto<PublicationDto>> EditAsync(string id, PublicationEditDto dto);
        Task<ResponseDto<FeedbackDto>> EditFeedbackAsync(string feedbackId, FeedbackEditDto dto);
        Task<ResponseDto<PublicationDto>> GetByIdAsync(string id);

        Task<ResponseDto<PaginationDto<List<PublicationDto>>>> GetEnabledListAsync(string searchTerm = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<List<FeedbackDto>>> GetFeedbacksAsync(string publicationId);
        Task<ResponseDto<object>> UploadImageAsync(string publicationId, IFormFile file);
    }
}
