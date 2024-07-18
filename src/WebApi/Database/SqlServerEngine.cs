namespace Monday.WebApi.Database;

using Dapper;
using Microsoft.Data.SqlClient;
using Monday.WebApi.Database.Interfaces;

internal sealed class SqlServerEngine : IDBEngine
{
    public async Task<int> ExecuteAsync(string connectionString, string command, object? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteAsync(command, parameters);
    }

    public async Task<TResult?> ExecuteScalarAsync<TResult>(string connectionString, string query, object? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<TResult>(query, parameters);
    }

    public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string connectionString, string query, object? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        return connection.Query<TResult>(query, parameters);
    }
}
