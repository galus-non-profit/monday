namespace Monday.WebApi.SignalR.Models;

internal sealed record AddBookStatus
{
    public required string ISBN { get; init; }
    public required string Name { get; init; }
    public required string Status { get; init; }
}
