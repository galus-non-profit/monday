namespace Monday.WebApi.Database.Interfaces;

public interface IDBEngine
{
    Task<int> ExecuteAsync(string connectionString, string command, object? parameters = null, CancellationToken cancellationToken = default);
}