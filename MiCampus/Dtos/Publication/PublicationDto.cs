using MiCampus.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.Publication
{
    public class PublicationDto
    {
        public string UserId { get; set; }

        public string TypeId { get; set; }

        public string Text { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateModify { get; set; }
    }
}
