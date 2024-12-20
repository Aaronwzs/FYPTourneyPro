namespace FYPTourneyPro.Services.Dtos.Chat
{
    public class ChatRoomDto
    {
        public Guid Id { get; set; }  // Unique identifier for the chat room
        public string Name { get; set; }  // The name of the chat room
        public List<ChatMessageDto> Messages { get; set; }  // A list of messages in the chat room

        public bool isDuplicate { get; set; }
    }
}
