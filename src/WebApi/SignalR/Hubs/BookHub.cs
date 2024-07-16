namespace Monday.WebApi.SignalR.Hubs;

using Microsoft.AspNetCore.SignalR;

public sealed class BookHub : Hub
{
    public static readonly string Pattern = "/bookHub";
}