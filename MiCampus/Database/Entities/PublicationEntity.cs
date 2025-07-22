using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("publications")]
    public class PublicationEntity : BaseEntity
    {
        [Column("id_user")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [Column("id_type")]
        public string TypeId { get; set; }

        [ForeignKey("TypeId")]
        public PublicationTypeEntity Type { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("date_create")]
        public DateTime DateCreate { get; set; }

        [Column("date_modify")]
        public DateTime DateModify { get; set; }
    }
}
