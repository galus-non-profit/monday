namespace Monday.WebApi.Services;

using Monday.WebApi.Interfaces;

public class BookReadService : IBookReadService
{
    public async Task<bool> IsExists(string isbn, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(false);
    }
}