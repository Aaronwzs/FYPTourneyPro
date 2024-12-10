using System;
using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class TournamentDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string HotlineNum { get; set; }
        public Boolean isMalaysian { get; set; }
        public string RulesAndRegulations { get; set; }

    }
}
