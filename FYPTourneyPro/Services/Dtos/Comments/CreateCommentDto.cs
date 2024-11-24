namespace FYPTourneyPro.Services.Dtos.Comments
{
    public class CreateCommentDto
    {

        public Guid PostId { get; set; }
        public string Content { get; set; }
        public Guid CreatedByUserId { get; set; }
    
    }
}
