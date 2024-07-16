namespace Monday.WebApi.Events;

using MediatR;

public sealed record UserNotDeleted : INotification
{
    public required Guid Id { get; init; }
}
