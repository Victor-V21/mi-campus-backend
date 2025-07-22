using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{

    [Table("content_images")]
    public class ContentImageEntity : BaseEntity
    {
        
        [Column("image_url")]
        public string ImageUrl { get; set; }

        // Foreign key to the ContentEntity
        [Column("content_id")]
        public string ContentId { get; set; }
        [ForeignKey("ContentId")]
        public virtual ContentEntity Content { get; set; }
    }
}
