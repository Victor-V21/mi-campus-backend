using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("subjects_taken")]
    public class SubjectTakenEntity : BaseEntity
    {
        [Column("year")]
        public int Year { get; set; }

        [Column("period")]
        public int Period { get; set; }

        [Column("grade")]
        public int Grade { get; set; }

        // Foreign key to the SubjectEntity
        [Column("user_id")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; }

        [Column("subject_id")]
        public string SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual SubjectEntity Subject { get; set; }

    }
}
