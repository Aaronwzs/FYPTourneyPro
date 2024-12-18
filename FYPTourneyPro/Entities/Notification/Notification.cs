using Volo.Abp.Domain.Entities.Auditing;

namespace FYPTourneyPro.Entities.Notification
{
    public class Notification : CreationAuditedEntity<Guid>
    {
        public string Type { get; set; }       // Type: ChatMessage, TournamentUpdate, etc.
        public Guid UserId { get; set; }
        public string Content { get; set; }    // Notification content/message
        public Guid RelatedEntityId { get; set; } // Links to ChatRoomId, TournamentId, etc.
        public bool IsRead { get; set; }       // Mark if the user has seen the notification
    }
}
