namespace MiCampus.Dtos.Chats
{
    public class ChatCreateDto
    {
        public string EmisorId { get; set; }
        public string ReceptorId { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool Received { get; set; }
        public bool Seen { get; set; }
    }
}