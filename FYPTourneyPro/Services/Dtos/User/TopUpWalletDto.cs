using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.User
{
    public class TopUpWalletDto : EntityDto<Guid>
    {

        public Guid UserId { get; set; }
        public decimal Amount { get; set; }


    }
}
