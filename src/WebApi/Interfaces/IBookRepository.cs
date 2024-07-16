namespace Monday.WebApi.Interfaces;

using Monday.WebApi.Domain;

public interface IBookRepository
{
    Task CreateAsync(BookEntity entity, CancellationToken cancellationToken = default);
}
