using System.ComponentModel.DataAnnotations;

namespace MiCampus.Dtos.Security.Users
{
    public class NormalUserCreateDto
    {
        [Display(Name = "first_name")]
        public string FirstName { get; set; }

        [Display(Name = "last_name")]
        public string LastName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El {0} es requerido")]
        [EmailAddress(ErrorMessage = "El {0} no tiene un formato de correo válido")]
        [StringLength(256, ErrorMessage = "El {0} no puede tener más de {1} caracteres")]
        public string Email { get; set; }

        [Display(Name = "no_account")]
        public string NoAccount { get; set; }

        [Display(Name = "avatar_url")]
        public string AvatarUrl { get; set; }

        [Display(Name = "birth_date")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "id_campus")]
        public string CampusId { get; set; }

        //public List<string> Roles { get; set; } = new();

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [StringLength(100, ErrorMessage = "La {0} no puede tener más de {1} caracteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no son iguales")]
        public string ConfirmPassword { get; set; }
    }
}