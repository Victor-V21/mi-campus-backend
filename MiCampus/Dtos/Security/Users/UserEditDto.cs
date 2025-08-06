namespace MiCampus.Dtos.Security.Users
{
    public class UserEditDto : UserCreateDto
    {
        public bool ChangePassword { get; set; }    // true or false, for change password
        public bool isEnabled { get; set; }
    }
}
