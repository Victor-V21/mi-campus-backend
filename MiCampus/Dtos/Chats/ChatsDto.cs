using Humanizer;
using MiCampus.Database.Entities;

namespace MiCampus.Dtos.Chats
{
    public class ChatsDto
    {
        public string EmisorId { get; set; }
        public string ReceptorId { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; } = DateTime.Now;
        public bool Received { get; set; }
        public bool Seen { get; set; }
    }
}