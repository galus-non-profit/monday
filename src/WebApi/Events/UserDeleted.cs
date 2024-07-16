namespace Monday.WebApi.Events;

using MediatR;

public sealed record UserDeleted : INotification
{
    public required Guid Id { get; init; }
}
