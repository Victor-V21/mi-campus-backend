using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;
using MiCampus.Database.Entities;
namespace MiCampus.Database.Entities
{
    [Table("careers_subjects_requisites")]
    public class CareerSubjectRequisiteEntity : BaseEntity
    {
        [Column("id_career_subject")]
        public string CareerSubjectId { get; set; }
        [ForeignKey("CareerSubjectId")]
        public CareerSubjectEntity CareerSubject { get; set; }

        [Column("id_requisite_subject")]
        public string RequisiteSubjectId { get; set; }
        [ForeignKey("RequisiteSubjectId")]
        public SubjectEntity RequisiteSubject { get; set; }
    }
}

