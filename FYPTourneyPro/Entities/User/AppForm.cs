using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.User
{
    public class AppForm : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; } // "organizer" or "referee"
        public string Description { get; set; }
        public bool IsApproved { get; set; } // Approval status
        public bool IsRejected { get; set; } // Rejection status
    }
}
