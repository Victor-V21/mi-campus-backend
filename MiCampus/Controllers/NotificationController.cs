using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly ICampusesServices _campusesServices;
        public NotificationController(
            ICampusesServices campusesServices
        )
        {
            _campusesServices = campusesServices;
        }
    }
}
