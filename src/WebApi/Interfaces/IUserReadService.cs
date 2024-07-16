namespace Monday.WebApi.Interfaces;

public interface IUserReadService
{
    Task<bool> IsExists(string name, CancellationToken cancellationToken = default);
}
