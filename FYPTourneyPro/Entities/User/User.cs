using Volo.Abp.Identity;
using System;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.User
{
    public class User : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
    }
}
