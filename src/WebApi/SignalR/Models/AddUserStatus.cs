namespace Monday.WebApi.SignalR.Models;


internal sealed record AddUserStatus
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Status { get; init; }
}