namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RegistrationId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? PairId { get; set; }
        public int Seed { get; set; }
        public int Points { get; set; }
    }
}
