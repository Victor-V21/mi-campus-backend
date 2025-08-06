using MiCampus.Database;
using MiCampus.Services.Interfaces;

namespace MiCampus.Services
{
    public class PublicationServices : IPublicationServices
    {
        private readonly CampusDbContext _context;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public PublicationServices(
            CampusDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }
    }
}
