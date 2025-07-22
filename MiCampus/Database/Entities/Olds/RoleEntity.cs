using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Database.Entities
{
    public class RoleEntity : IdentityRole
    {
        [Column("description")]
        [StringLength(256)]
        public string Description { get; set; }
    }
}