using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.User
{
    public class AppFormDto : EntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
    }
}
