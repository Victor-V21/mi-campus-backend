namespace MiCampus.Dtos.Feedback
{
    public class FeedbackDto
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
