namespace Monday.WebApi.SignalR.Models;

internal sealed record DeleteUserStatus
{
    public required Guid Id { get; init; }
    public required string Status { get; init; }
}
