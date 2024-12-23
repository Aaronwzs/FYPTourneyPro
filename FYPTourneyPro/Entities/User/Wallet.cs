using FYPTourneyPro.Services.Dtos.User;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.User
{
    public class Wallet : Entity<Guid>
    {
        public Guid UserId { get; set; }
       public decimal Balance { get; set; } = 0m;

      
    }
}
