namespace Monday.WebApi.Database.Interfaces;

public interface IDBEngine
{
    Task<int> ExecuteAsync(string connectionString, string command, object? parameters = null, CancellationToken cancellationToken = default);
    Task<TResult?> ExecuteScalarAsync<TResult>(string connectionString, string query, object? parameters = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> QueryAsync<TResult>(string connectionString, string query, object? parameters = null, CancellationToken cancellationToken = default);
}
