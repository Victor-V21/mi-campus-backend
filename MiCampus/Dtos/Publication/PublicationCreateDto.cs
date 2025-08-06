using System.ComponentModel.DataAnnotations.Schema;
using MiCampus.Database.Entities;

namespace MiCampus.Dtos.Publication
{
    public class PublicationCreateDto
    {
      
        public string UserId { get; set; }

        public string TypeId { get; set; }

        public string Title { get; set; }


        public string Text { get; set; }


    }
}
