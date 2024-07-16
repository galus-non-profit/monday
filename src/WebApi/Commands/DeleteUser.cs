namespace Monday.WebApi.Commands;

using MediatR;

public sealed record DeleteUser : IRequest
{
    public required Guid Id { get; init; }
}
