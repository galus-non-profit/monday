namespace Monday.WebApi.Events;

using MediatR;

public sealed record UserNotAdded : INotification
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}