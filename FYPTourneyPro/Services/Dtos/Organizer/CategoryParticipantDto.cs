using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class CategoryParticipantDto : EntityDto<Guid>
    {
        public int Seed { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PlayerRegistrationId { get; set; }
        public string CategoryName { get; set; } // Category Name for display
        public string PlayerName { get; set; }  // Player Name for display
    }
}
