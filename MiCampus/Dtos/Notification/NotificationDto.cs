using MiCampus.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiCampus.Dtos.Notification
{
    public class NotificationDto
    {
        public string UserId { get; set; }
        public string TypeId { get; set; }
        public string Text { get; set; }

        public bool Seen { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModify { get; set; }
    }
}
