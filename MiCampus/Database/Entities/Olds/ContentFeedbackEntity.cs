using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{

    [Table("contents_feedbacks")]
    public class ContentFeedbackEntity : BaseEntity
    {
        
        
        [Column("comment")]
        public string Comment { get; set; }

        [Column("rating")]
        public int? Rating { get; set; }

        [Column("content_id")]

        // Foreign key to the ContentEntity
        public string ContentId { get; set; }

        [ForeignKey("ContentId")]
        public virtual ContentEntity Content { get; set; }

        // Foreign key to the UserEntity
        [Column("user_id")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }
    }
}
