namespace MiCampus.Dtos.Subjects
{
    public class SubjectActionResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        //requisitos de la materia
        public List<string> Requisites { get; set; }
    }
}
