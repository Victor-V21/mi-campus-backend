using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;

namespace MiCampus.Database.Entities
{
    [Table("campuses")]
    public class CampusEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
