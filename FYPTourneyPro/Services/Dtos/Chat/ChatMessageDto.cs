using FYPTourneyPro.Entities.Chatroom;

namespace FYPTourneyPro.Services.Dtos.Chat
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }  // Unique identifier for the message
        public Guid ChatRoomId { get; set; }  // Nullable, ID of the chat room if it's a group message
        public string Content { get; set; }  // The content of the message
        public bool IsSeen { get; set; }  // Indicates if the message has been seen
        public Guid CreatorId { get; set; }  // Added to know who sent the message
        public string Username { get; set; }
        public DateTime CreationTime { get; set; }  // Added to track when the message was sent
        public string ChatRoomName { get; set; }
    }
}
