using Volo.Abp.Identity;
using System;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.UserM
{
    public class CustomUser : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator IdentityUser(CustomUser v)
        {
            throw new NotImplementedException();
        }
    }
}
