namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class MatchDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public int round {  get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int courtNum { get; set; }
    }
}
