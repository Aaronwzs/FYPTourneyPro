using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace FYPTourneyPro.Entities.DiscussionBoard
{ 
        public class Post : Entity<Guid>
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public Guid CreatedByUserId { get; set; }
            public int Upvotes { get; set; }
            public int Downvotes { get; set; }
            public DateTime CreationTime { get; set; }

            public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

     public class Comment : Entity<Guid>
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; }
        public Guid CreatedByUserId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }

}
