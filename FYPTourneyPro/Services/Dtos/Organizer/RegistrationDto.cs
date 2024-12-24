namespace FYPTourneyPro.Services.Dtos.Organizer
{
    public class RegistrationDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
       
        public DateTime RegDate { get; set; }
        public double TotalAmount { get; set; }
        public Guid UserId1 { get; set; }
        public Guid? UserId2 { get; set; }

        public string UserName1 {  get; set; }
        public string? UserName2 { get; set; }

        public Guid tournamentId {  get; set; }

        public Guid organizerId { get; set; }
        public string? CategoryName { get; set; }

        public string? FullName1 { get; set; }
        public string? FullName2 { get; set;}
    }
}
