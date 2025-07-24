using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.Security.Users
{
    public class UserActionResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NoAccount { get; set; }
        public string AvatarUrl { get; set; }
        public DateOnly BirthDay { get; set; }
        public string CampusId { get; set; }
        public List<string> Roles { get; set; }
    }
}
