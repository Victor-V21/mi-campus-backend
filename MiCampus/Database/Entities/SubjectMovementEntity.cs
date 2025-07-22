using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("subjects_movements")]
    public class SubjectMovementEntity : BaseEntity
    {
        [Column("movement")]
        public string Movement { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
