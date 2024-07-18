namespace Monday.WebApi.Events;

public sealed record class BookNotAdded : INotification
{
    public required string ISBN { get; init; }
    public required string Name { get; init; }
}
