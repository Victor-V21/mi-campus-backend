using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("careers_subjects")]
    public class CareerSubjectEntity : BaseEntity
    {
        [Column("id_career")]
        public string CareerId { get; set; }

        [ForeignKey("CareerId")]
        public CareerEntity Career { get; set; }

        [Column("id_clase")]
        public string SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public SubjectEntity Subject { get; set; }

        // Relación con los requisitos
        public ICollection<CareerSubjectRequisiteEntity> Requisites { get; set; }
    }
}

