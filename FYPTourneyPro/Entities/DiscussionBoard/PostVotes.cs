using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace FYPTourneyPro.Entities.DiscussionBoard
{
    public class PostVotes : CreationAuditedEntity<Guid>
    {
        public Guid PostId { get; set; } // Foreign Key for Posts
        public string VoteType { get; set; } // 'Upvote' or 'Downvote'
    }
}
