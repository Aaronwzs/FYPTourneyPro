namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class MatchParticipantDto
    {


        public Guid? Id { get; set; }
        public Guid? RegistrationId { get; set; }
        public Guid? userId { get; set; }
        public Guid? pairId { get; set; }
        public Guid? matchId { get; set; }
        public Boolean? isWinner { get; set; }
        public Guid CategoryId { get; set; }
        public string? userName { get; set; }
    }
}
