using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Persons.API.Database.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreateDate { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTime UpdateDate { get; set; }
    }
}