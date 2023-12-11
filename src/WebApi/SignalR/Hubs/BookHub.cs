using Microsoft.AspNetCore.SignalR;

namespace Monday.WebApi.SignalR.Hubs
{
    public sealed class BookHub : Hub
    {
        public static readonly string Pattern = "/bookHub";

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
