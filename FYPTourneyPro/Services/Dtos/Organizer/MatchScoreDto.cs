namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class MatchScoreDto
    {
        public Guid MatchParticipantId { get; set; }
        public Guid MatchId { get; set; }
        public Guid WinnerId { get; set; } // userid or pairid
        public string Set1Score { get; set; }
        public string Set2Score { get; set; }
        public string? Set3Score { get; set; }
    }
}
