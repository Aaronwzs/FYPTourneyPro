using FYPTourneyPro.Entities.Chatroom;
using Microsoft.AspNetCore.SignalR;


namespace FYPTourneyPro.Services.Chat
{
    public class Chathub    :   Hub
    {

        public async Task SendMessage(string chatRoomId, string userId, string message, DateTime creationTime)
        { 
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", userId, message, creationTime);
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
