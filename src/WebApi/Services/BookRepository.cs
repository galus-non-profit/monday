namespace Monday.WebApi.Services;

using Monday.WebApi.Domain;
using Monday.WebApi.Interfaces;

public sealed class BookRepository : IBookRepository
{
    public Task CreateAsync(BookEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}