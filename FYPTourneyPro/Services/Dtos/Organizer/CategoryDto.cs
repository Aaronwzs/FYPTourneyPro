using System;
using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class CategoryDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TournamentId { get; set; }
        public bool IsPair { get; set; }
    }
}
