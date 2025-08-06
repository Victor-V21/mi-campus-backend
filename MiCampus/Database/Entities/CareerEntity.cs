using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("careers")]
    public class CareerEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("id_grade")]
        public string IdGrade { get; set; }

        [ForeignKey("IdGrade")]
        public GradeEntity AcademicGrade { get; set; }

        // 🔹 Relación uno a muchos: Una carrera puede tener muchas materias
        public ICollection<CareerSubjectEntity> CareerSubjects { get; set; }
    }
}
