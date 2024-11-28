using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class MatchScore : Entity<Guid>
    {
        public Guid MatchParticipantId { get; set; }
        public int Set1Score { get; set; }
        public int Set2Score { get; set; }
        public int Set3Score { get; set; }
    }
}
