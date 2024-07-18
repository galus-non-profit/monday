namespace Monday.WebApi.Commands;

using Monday.WebApi.Interfaces;

internal sealed record AddUserFromJob : IRequest, IAddUser
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Password { get; init; }
}
