namespace MiCampus.Dtos.Careers
{
    public class CareerCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> IdsSubjects { get; set; }
    }
}
