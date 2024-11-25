namespace FYPTourneyPro.Services.Dtos.Comments
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public Guid CreatedByUserId { get; set; }
        public int? Upvotes { get; set; }
        public int? Downvotes { get; set; }
    }
}
