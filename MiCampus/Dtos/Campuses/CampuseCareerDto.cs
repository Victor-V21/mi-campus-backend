using MiCampus.Dtos.Careers;

namespace MiCampus.Dtos.Campuses
{
    public class CampuseCareerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<CareerActionResponseDto> Careers { get; set; }
    }
}