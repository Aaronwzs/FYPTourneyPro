using FYPTourneyPro.Services.Dtos.Comments;

namespace FYPTourneyPro.Services.Dtos.Posts
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Upvotes { get; set; }
        public int? Downvotes { get; set; }
        public DateTime CreationTime { get; set; }
        public List<CommentDto>? Comments { get; set; } = new List<CommentDto>();
    }
}
