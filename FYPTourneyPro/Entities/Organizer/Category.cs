
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.Organizer
{
    public class Category : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TournamentId { get; set; }
    }

}
