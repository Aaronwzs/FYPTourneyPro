using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace FYPTourneyPro.Entities.Chatroom
{
    public class ChatRoom : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public ICollection<ChatMessage> Messages { get; set; } // Navigation property to all messages in the chat room
    }

    public class ChatMessage : FullAuditedAggregateRoot<Guid>
    {
        public Guid? ChatRoomId { get; set; }  // Nullable: Links message to a chat room for group messages
        public Guid? ReceiverId { get; set; }  // Nullable: Used for direct messages to indicate the recipient
        public string Content { get; set; }
        public ChatType ChatType { get; set; }
        public bool IsSeen { get; set; }
        public ChatRoom ChatRoom { get; set; } // Navigation property back to ChatRoom
    }

    public enum ChatType
    {
        Direct,
        Group
    }

    public class ChatRoomParticipant : FullAuditedEntity<Guid>
    {
        public Guid ChatRoomId { get; set; }  // The ID of the chat room
        public Guid UserId { get; set; }       // The ID of the user

        public ChatRoom ChatRoom { get; set; } // Navigation property to the chat room

        public IdentityUser User;
    }
}

