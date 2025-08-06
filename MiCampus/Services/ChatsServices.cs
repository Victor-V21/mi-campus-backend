using MiCampus.Database;
using MiCampus.Database.Entities;
using MiCampus.Dtos.Common;
using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MiCampus.Services
{
    public class ChatsServices : IChatsServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public ChatsServices(
            CampusDbContext context,
            IConfiguration configuration,
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }
    }
}