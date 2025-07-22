using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("subject_users")]
    public class SubjectUserEntity : BaseEntity
    {
        [Column("id_subject")]
        public string SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public SubjectEntity Subject { get; set; }

        [Column("id_users")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [Column("id_movement")]
        public string MovementId { get; set; }

        [ForeignKey("MovementId")]
        public SubjectMovementEntity Movement { get; set; }
    }
}
