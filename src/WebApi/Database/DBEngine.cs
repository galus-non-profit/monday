namespace Monday.WebApi.Database;

using Dapper;
using Microsoft.Data.SqlClient;
using Monday.WebApi.Database.Interfaces;

public class DBEngine : IDBEngine
{
    public async Task<int> ExecuteAsync(string connectionString, string command, object? parameters = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        return await connection.ExecuteAsync(command, parameters);
    }
}