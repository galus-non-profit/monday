namespace Monday.WebApi.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;

public sealed class UserHub : Hub
{
    public static readonly string Pattern = "/userHub";
}
