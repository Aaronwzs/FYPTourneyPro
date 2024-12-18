namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class RegistrationDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
       
        public DateTime RegDate { get; set; }
        public double TotalAmount { get; set; }
        public Guid userId1 { get; set; }
        public Guid? userId2 { get; set; }

        public string userName1 {  get; set; }
        public string? userName2 { get; set; }

        public Guid tournamentId {  get; set; }

        public Guid organizerId { get; set; }
        public string? CategoryName { get; set; }
    }
}
