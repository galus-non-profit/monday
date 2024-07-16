namespace Monday.WebApi.Commands;

using MediatR;
using Monday.WebApi.Interfaces;

internal sealed record AddUserFromJob : IRequest, IAddUser
{
    public required string Email { get; init; }
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string PasswordHashed { get; init; }
}