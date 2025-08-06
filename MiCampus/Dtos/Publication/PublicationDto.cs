using MiCampus.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.Publication
{
    public class PublicationDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateCreate { get; set; }

        public string TypeId { get; set; }
        public string TypeName { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public List<PublicationImageEntity> Images { get; set; } = new();
        public List<FeedbackEntity> Feedbacks { get; set; } = new();
    }
}
