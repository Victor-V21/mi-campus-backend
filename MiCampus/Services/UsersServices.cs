using AutoMapper;
using MiCampus.Database;
using Microsoft.AspNetCore.Identity;

namespace MiCampus.Services
{
    public class UsersServices
    {
        private readonly UserManager<UserEntity> _userManager;

        private readonly CampusDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public UsersServices(CampusDbContext context, IMapper mapper, int pageSize = 10, int pageSizeLimit = 100)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            PAGE_SIZE = pageSize;
            PAGE_SIZE_LIMIT = pageSizeLimit;
        }



    }
}
