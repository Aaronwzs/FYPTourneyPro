using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Match : Entity<Guid>
    {
        public Guid CompeteId { get; set; } // Link to the compete match-up
        public DateTime MatchTime { get; set; }
        public string Court { get; set; } // Court name or number

        // Navigation properties
        public virtual Compete Compete { get; set; }
    }
}
