using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Participant : Entity<Guid>
    {
        public int Seed { get; set; }
        public int Points { get; set; }
        public Guid UserId { get; set; }
        public Guid RegistrationId { get; set; }

        public Guid TournamentId { get; set; }
        public Guid PairId { get; set; }

    }
}
