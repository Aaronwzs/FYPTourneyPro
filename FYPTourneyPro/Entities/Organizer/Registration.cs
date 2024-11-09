using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Registration : Entity<Guid>
    {
        public DateTime RegDate { get; set; }
        public double totalAmount { get; set; }
        public Guid CategoryId { get; set; }
        
    }
}
