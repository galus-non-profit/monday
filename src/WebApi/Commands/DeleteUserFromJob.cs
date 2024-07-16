namespace Monday.WebApi.Commands;

using MediatR;
using Monday.WebApi.Interfaces;

internal sealed record DeleteUserFromJob : IRequest, IDeleteUser
{
    public Guid Id { get; init; }
}
