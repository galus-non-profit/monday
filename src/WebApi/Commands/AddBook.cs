namespace Monday.WebApi.Commands;

using MediatR;

public sealed record class AddBook : IRequest
{
    public required string ISBN { get; init; }
    public required string Name { get; init; }
}
