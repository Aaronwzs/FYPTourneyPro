using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Compete : Entity<Guid>
    {
        public Guid CategoryId { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set;}

        // Navigation properties ( To get CategoryId, and 2 players from category Participant
        public virtual Category Category { get; set; }
        public virtual CategoryParticipant Player1 { get; set; }
        public virtual CategoryParticipant Player2 { get; set; }
    }
}
