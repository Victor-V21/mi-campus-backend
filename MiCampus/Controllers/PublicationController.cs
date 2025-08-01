using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/publication")]
    public class PublicationController
    {
        private readonly ICampusesServices _campusesServices;
        public PublicationController(
            ICampusesServices campusesServices
        )
        {
            _campusesServices = campusesServices;
        }
    }
}
