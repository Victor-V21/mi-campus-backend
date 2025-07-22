using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Micampus.Database.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace MiCampus.Database.Entities
{
    
    public class RoleEntity : IdentityRole
    {
        [Column("description")]
        [StringLength(256)]
        public string Description { get; set; }
    }
}
