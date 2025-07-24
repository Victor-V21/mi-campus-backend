namespace MiCampus.Dtos.Security.Users
{
    public class UserDto // utilizado para lectura de datos de la base de datos
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NoAccount { get; set; }
        public string AvatarUrl { get; set; }
        public DateOnly BirthDay { get; set; }
        public string CampusId { get; set; }
        public List<string> Roles { get; set; }

    }
}
