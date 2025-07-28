using MiCampus.Services.Interfaces;

namespace MiCampus.Services
{
    public class AuditService : IAuditService
    {
        private readonly HttpContext _httpContext;

        public AuditService(
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string GetUserId()
        {
            var userIdClaim = _httpContext.User.Claims
                .Where(x => x.Type == "UserId").FirstOrDefault();

            return userIdClaim.Value ?? "";
        }
    }
}
