using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class MatchScore : Entity<Guid>
    {
        public Guid MatchParticipantId { get; set; }
        public int NumOfSets { get; set; } // 2 or 3
        public int Score { get; set; }
       

    }
}
