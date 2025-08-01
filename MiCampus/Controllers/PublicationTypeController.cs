using MiCampus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiCampus.Controllers
{
    [ApiController]
    [Route("api/publication-type")]
    public class PublicationTypeController
    {
        private readonly ICampusesServices _campusesServices;
        public PublicationTypeController(
            ICampusesServices campusesServices
        )
        {
            _campusesServices = campusesServices;
        }
    }
}
