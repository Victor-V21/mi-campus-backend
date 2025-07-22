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

        [Column("grade")]
        public string Grade { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
