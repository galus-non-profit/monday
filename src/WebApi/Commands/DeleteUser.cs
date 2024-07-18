namespace Monday.WebApi.Commands;

public sealed record DeleteUser : IRequest
{
    public required Guid Id { get; init; }
}
