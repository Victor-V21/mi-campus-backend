using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("publications_images")]
    public class PublicationImageEntity : BaseEntity
    {
        [Column("id_publication")]
        public string PublicationId { get; set; }

        [ForeignKey("PublicationId")]
        public PublicationEntity Publication { get; set; }

        [Column("file_name")]
        public string FileName { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("date_upload")]
        public DateTime DateUpload { get; set; }
    }
}
