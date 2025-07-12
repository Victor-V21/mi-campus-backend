using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("university_careers")]
    public class UniversityCareerEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<UserEntity> Users { get; set; }
        public ICollection<SubjectEntity> Subjects { get; set; }
        public ICollection<CampusEntity> Campuses { get; set; }
    }
}
