namespace FYPTourneyPro.Services.Dtos.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public Guid RelatedEntityId { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
       
    }
}
