using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persons.API.Dtos.Security.Users
{
    public class UserActionResponseDto
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Column("avatar_url")]
        [StringLength(256)]
        public string AvatarUrl { get; set; }
        public DateTime BirthDay { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
