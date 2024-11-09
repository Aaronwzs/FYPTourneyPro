using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class MatchParticipant : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid PairId { get; set; }
        public Guid MatchId { get; set; }
        public Boolean IsWinner { get; set; }

    }
}
