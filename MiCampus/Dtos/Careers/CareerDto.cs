namespace MiCampus.Dtos.Careers
{
    public class CareerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> IdsSubjects { get; set; }
    }
}
