using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("campuses_careers")]
    public class CampusCareerEntity : BaseEntity
    {
        [Column("id_campus")]
        public string CampusId { get; set; }

        [ForeignKey("CampusId")]
        public CampusEntity Campus { get; set; }

        [Column("id_career")]
        public string CareerId { get; set; }

        [ForeignKey("CareerId")]
        public CareerEntity Career { get; set; }
    }
}
