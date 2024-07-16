namespace Monday.WebApi.Commands;

using MediatR;

public sealed record AddUser : IRequest
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}