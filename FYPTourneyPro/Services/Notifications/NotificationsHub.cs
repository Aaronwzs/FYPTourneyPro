using Microsoft.AspNetCore.SignalR;

namespace FYPTourneyPro.Services.Notifications
{
    public class NotificationsHub : Hub<INotificationsHub>
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

    }

    public interface INotificationsHub
    {
        Task SendNotification(string message);
    }
}
