namespace MiCampus.Dtos.Careers
{
    public class CareerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // referencia a la tabla de grados academicos
        public string IdGrade { get; set; }
        public string Description { get; set; }
        public List<AddSubjectDto> Subjects { get; set; }
    }
}