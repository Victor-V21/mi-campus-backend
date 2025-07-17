using System.ComponentModel.DataAnnotations.Schema;
using MiCampus.Database.Entities;

namespace MiCampus.Dtos.Campuses
{
    public class CampusesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> ListCareersIds { get; set; }
    }
}
