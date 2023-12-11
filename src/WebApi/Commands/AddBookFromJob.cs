using MediatR;

namespace Monday.WebApi.Commands
{
    internal sealed record class AddBookFromJob : IRequest
    {
        public required string Name { get; init; }
    }
}
