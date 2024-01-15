using Monday.WebApi.Domain;

namespace Monday.WebApi.Interfaces
{
    public interface IBookRepository
    {
        Task CreateAsync(BookEntity entity, CancellationToken cancellationToken = default);
    }
}
