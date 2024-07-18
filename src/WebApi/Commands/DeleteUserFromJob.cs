namespace Monday.WebApi.Commands;

using Monday.WebApi.Interfaces;

internal sealed record DeleteUserFromJob : IRequest, IDeleteUser
{
    public Guid Id { get; init; }
}
