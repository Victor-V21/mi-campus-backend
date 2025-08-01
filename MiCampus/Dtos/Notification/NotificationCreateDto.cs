using MiCampus.Database.Entities;

namespace MiCampus.Dtos.Notification
{
    public class NotificationCreateDto
    {
        public string UserId { get; set; }
        public string TypeId { get; set; }
        public string Text { get; set; }

        public bool Seen { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModify { get; set; }
    }
}
