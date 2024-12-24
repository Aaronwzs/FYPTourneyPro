using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FYPTourneyPro.Services.Dtos.Posts
{
    public class PostVoteDto
    {

        public Guid Id { get; set; } // Unique identifier for the vote
        public Guid PostId { get; set; } // Foreign key to Post
        public string Title { get; set; }
        public string Content { get; set; }
        public string VoteType { get; set; } // 'Upvote' or 'Downvote'
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int Score { get; set; }
        public DateTime CreationTime { get; set; } // Timestamp
        public Guid CreatorId { get; set; } //Creator id    

        public string Creator { get; set; }
    }
}
