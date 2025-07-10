using System.ComponentModel.DataAnnotations;

namespace MiCampus.Dtos.Security.Roles
{
    public class RoleCreateDto
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatario")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1}")]
        public string Name { get; set; }

        [Display(Name = "Descripcion")]
        [StringLength(256, ErrorMessage = "El campo {0} no puede tener mas de {1}")]
        public string Description { get; set; }
    }
}
