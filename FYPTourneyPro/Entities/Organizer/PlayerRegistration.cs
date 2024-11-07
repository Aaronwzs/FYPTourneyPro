using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class PlayerRegistration : Entity<Guid>
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }

        public string UserName { get; set; } = string.Empty; //Added userName

    }
}
