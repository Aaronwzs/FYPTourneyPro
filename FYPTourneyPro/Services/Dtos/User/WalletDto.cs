using FYPTourneyPro.Entities.UserM;
using Volo.Abp.Application.Dtos;

namespace FYPTourneyPro.Services.Dtos.User
{
    public class WalletDto : EntityDto<Guid>
    {

        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal TopUpAmount { get; set; }

        public decimal WithdrawAmount { get; set; }

        public decimal RegFee { get; set;}
    }
}
