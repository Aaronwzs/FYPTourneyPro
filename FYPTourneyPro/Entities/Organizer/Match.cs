using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Match : Entity<Guid>
    {
        public string round { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int courtNum { get; set; }
        
        public Guid CategoryId { get; set; }

    }
}
