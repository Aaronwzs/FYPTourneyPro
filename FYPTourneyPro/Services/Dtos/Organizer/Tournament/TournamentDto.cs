﻿using System;
using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.Organizer.Tournament
{
    public class TournamentDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}