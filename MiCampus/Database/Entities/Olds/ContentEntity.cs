using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("contents")]
    public class ContentEntity : BaseEntity
    {
        

        [Column("title")]
        public string Title { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("content_type")]
        public string ContentType { get; set; } // event, post, etc.

        [Column("event_date")]
        public DateTime? EventDate { get; set; }

        [Column("event_time")]
        public TimeSpan? EventTime { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("map_location")]
        public string MapLocation { get; set; }

        [Column("cover_image")]
        public string CoverImage { get; set; }

        [Column("event_status")]
        public string EventStatus { get; set; }

        [Column("is_moderated")]
        public bool IsModerated { get; set; }

        // Foreign key to the UserEntity
        [Column("user_id")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }

        //Dependencias
        public ICollection<ContentFeedbackEntity> Feedbacks { get; set; }
        public ICollection<ContentImageEntity> Images { get; set; }
    }
}
