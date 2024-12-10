using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Tournament : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RegStartDate { get; set; }
        public DateTime RegEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // New UserId property to track which user created the tournament
        public Guid UserId { get; set; }

        //Additional Fields
        
        public string HotlineNum { get; set; }
        public string RulesAndRegulations { get; set; }
        public bool IsMalaysian { get; set; }


    }
}
