namespace MiCampus.Dtos.Campuses
{
    public class CampusesCreateDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        // List of career IDs associated with the campus
        public List<string> CareersIds { get; set; } = new List<string>();

    }
}
