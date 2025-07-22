using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.Subjects
{
    public class SubjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        //requisitos de la materia
        public List<string> Requisites { get; set; }

        //carreras a la que pertenece
        public List<string> CareerId { get; set; }
    }
}
