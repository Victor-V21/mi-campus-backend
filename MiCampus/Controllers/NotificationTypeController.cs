using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{

    [ApiController]
    [Route("api/notification-type")]
    public class NotificationTypeController : ControllerBase
    {
        private readonly ICampusesServices _campusesServices;
        public NotificationTypeController(
            ICampusesServices campusesServices
        )
        {
            _campusesServices = campusesServices;
        }
    }
}
