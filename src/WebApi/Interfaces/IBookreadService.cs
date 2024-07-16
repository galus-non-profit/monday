namespace Monday.WebApi.Interfaces;

public interface IBookReadService
{
    Task<bool> IsExists(string isbn, CancellationToken cancellationToken = default);
}
