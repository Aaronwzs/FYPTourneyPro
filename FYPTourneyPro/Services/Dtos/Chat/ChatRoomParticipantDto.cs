namespace FYPTourneyPro.Services.Dtos.Chat
{
    public class ChatRoomParticipantDto
    {
        public Guid Id { get; set; }  // Unique identifier for the participant
        public string Name { get; set; }

        public Guid ChatRoomId { get; set; }  // The ID of the chat room the participant is in
        public Guid UserId { get; set; }  // The ID of the user participating in the chat room
        public string? LastMessage { get; set; }
    
        public string? Username { get; set; }

        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; internal set; }
        public Guid? CurrentUserId { get; internal set; }
    }

}
