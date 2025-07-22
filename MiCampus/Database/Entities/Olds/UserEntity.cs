using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        [Column("first_name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Column("account_number")]
        [StringLength(11)]
        public string AccountNumber { get; set; }

        [Column("avatar_url")]
        [StringLength(256)]
        public string AvatarUrl { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("registration_date")]
        public DateTime RegistrationDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("career_id")]
        public string CareerId { get; set; }
        public UniversityCareerEntity Career { get; set; }
    }
}
