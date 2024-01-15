using Monday.WebApi.Domain;
using Monday.WebApi.Interfaces;

namespace Monday.WebApi.Services
{
    public sealed class BookRepository : IBookRepository
    {
        public async Task CreateAsync(BookEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
