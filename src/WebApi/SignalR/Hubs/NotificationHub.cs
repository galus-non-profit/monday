using Microsoft.AspNetCore.SignalR;

namespace Monday.WebApi.SignalR.Hubs
{
    public sealed class NotificationsHub : Hub
    {
        public static readonly string Pattern = "/notificationsHub";
        public async Task SendNotification(string content)
        {
            await Clients.All.SendAsync("ReceiveNotification", content);
        }
    }
}
