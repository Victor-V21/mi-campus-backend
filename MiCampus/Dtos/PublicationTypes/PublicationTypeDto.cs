using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.PublicationTypes
{
    public class PublicationTypeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
