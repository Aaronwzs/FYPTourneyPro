using FYPTourneyPro.Entities.Organizer;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Services.Dtos.Organizer.PlayerRegistration
{
    public class PlayerRegDto : EntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }
        public string UserName { get; set; }
    }
}
