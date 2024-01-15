using MediatR;
using Monday.WebApi.Interfaces;

namespace Monday.WebApi.Commands
{
    internal sealed record class AddBookFromJob : IRequest, IAddBook
    {
        public required string ISBN { get; init; }
        public required string Name { get; init; }
    }
}
