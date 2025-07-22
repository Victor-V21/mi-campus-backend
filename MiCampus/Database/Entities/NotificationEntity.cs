using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("notifications")]
    public class NotificationEntity : BaseEntity
    {
        [Column("id_user")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [Column("id_type")]
        public string TypeId { get; set; }

        [ForeignKey("TypeId")]
        public NotificationTypeEntity Type { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("seen")]
        public bool Seen { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; }

        [Column("date_modify")]
        public DateTime DateModify { get; set; }
    }
}
