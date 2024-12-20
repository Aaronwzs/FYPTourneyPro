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
            var changedRoomId = chatRoomId.ToString();

            await Clients.Group(changedRoomId).SendAsync("ReceiveMessage", userId, message, creationTime);

            await _notificationAppService.SaveChatNotification(chatRoomId, userId);
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
