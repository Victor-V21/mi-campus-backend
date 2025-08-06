using Micampus.Database.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Database.Entities
{
    [Table("publications_feedback")]
    public class FeedbackEntity : BaseEntity
    {
        public string PublicationId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }  
        public DateTime CreatedAt { get; set; }

        public PublicationEntity Publication { get; set; }
        public UserEntity User { get; set; }
    }

}
