using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("types_grades")]
    public class GradeEntity : BaseEntity
    {
        [Column("name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}