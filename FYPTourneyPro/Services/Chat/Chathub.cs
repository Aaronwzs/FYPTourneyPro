using FYPTourneyPro.Entities.Chatroom;
using FYPTourneyPro.Services.Notifications;
using Microsoft.AspNetCore.SignalR;


namespace FYPTourneyPro.Services.Chat
{

    public class Chathub : Hub
    {
        private readonly NotificationAppService _notificationAppService;

        public Chathub(NotificationAppService notificationAppService)
        {
            _notificationAppService = notificationAppService;
        }

        public async Task SendMessage(Guid chatRoomId, Guid userId, string message, DateTime creationTime)
        {
            await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", userId, message, creationTime);

            // 2. Notify others in the group (excluding the sender)
            var notificationMessage = $"New message from {userId} in room {chatRoomId} at {creationTime}";

            await _notificationAppService.SaveChatNotification(chatRoomId, userId, notificationMessage);

            await Clients.OthersInGroup(chatRoomId.ToString()).SendAsync("ReceiveNotification", notificationMessage);
        }

        public async Task JoinRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task LeaveRoom(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }
    }
}
