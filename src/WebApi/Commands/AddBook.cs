using MediatR;

namespace Monday.WebApi.Commands;

public sealed record class AddBook : IRequest
{
     public required string ISBN { get; init; }
     public required string Name { get; init; }
}