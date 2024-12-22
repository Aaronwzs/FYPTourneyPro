using FYPTourneyPro.Entities.Chatroom;
using FYPTourneyPro.Services.Dtos.Chat;
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

        public async Task SendMessage(ChatMessageDto input)
        {
            var changedRoomId = input.ChatRoomId.ToString();

            await Clients.Group(changedRoomId).SendAsync("ReceiveMessage", input.Username, input.Content, input.CreationTime);

            await _notificationAppService.SaveChatNotification(input.ChatRoomId, input.CreatorId);
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
