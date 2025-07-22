namespace MiCampus.Dtos.Subjects
{
    public class SubjectCreateDto
    {
        public string Name { get; set; }
        public string Code { get; set; }

        //requisitos de la materia
        public List<string> IdsRequisites { get; set; }
    }
}
