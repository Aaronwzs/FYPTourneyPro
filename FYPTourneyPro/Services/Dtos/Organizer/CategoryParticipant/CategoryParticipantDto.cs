﻿using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Services.Dtos.Organizer.CategoryParticipant
{
    public class CategoryParticipantDto : EntityDto<Guid>
    {
        public int Seed { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PlayerRegistrationId { get; set; }
    }
}
