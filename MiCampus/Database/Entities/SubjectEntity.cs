using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("subjects")]
    public class SubjectEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("code")]
        public string Code { get; set; }


        // llave foránea a la carrera
        [Column("career_id")]
        public string CareerId { get; set; }
        [ForeignKey("CareerId")]
        public virtual UniversityCareerEntity Career { get; set; }

        // llave foránea a la materia que es requisito
        [Column("prerequisite_id")]
        public string RequisiteId { get; set; }
        [ForeignKey("RequisiteId")]
        public virtual SubjectEntity Requisite { get; set; }


        public ICollection<SubjectTakenEntity> SubjectTaken { get; set; }
        public ICollection<SubjectEntity> Requisites { get; set; }


    }
}
