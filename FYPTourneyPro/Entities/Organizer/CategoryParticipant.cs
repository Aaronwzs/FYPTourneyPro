using System;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class CategoryParticipant : Entity<Guid>
    {
        public int Seed { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PlayerRegistrationId { get; set; }
    }
}
