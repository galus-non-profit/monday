namespace Monday.WebApi.Services;

using Monday.WebApi.Database.Interfaces;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Options;

internal sealed class UserReadService : IUserReadService
{
    private const string CHECK_IS_USER_NAME_EXISTS_QUERY = """
                                                           SELECT
                                                             CASE WHEN EXISTS(SELECT * FROM [dbo].[Users]
                                                             WHERE [UserName] = @Name)
                                                             THEN 1 ELSE 0 END AS Result;
                                                           """;

    private readonly IDBEngine dbEngine;
    private readonly ILogger<UserReadService> logger;
    private readonly SqlOptions sqlOptions;

    public UserReadService(IDBEngine dbEngine, SqlOptions sqlOptions, ILogger<UserReadService> logger)
    {
        this.dbEngine = dbEngine;
        this.sqlOptions = sqlOptions;
        this.logger = logger;
    }

    public async Task<bool> IsExists(string name, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try check is user exists");

        var parameters = new
        {
            Name = name,
        };

        var isExists = await this.dbEngine.ExecuteScalarAsync<bool>(this.sqlOptions.SqlConnectionString, CHECK_IS_USER_NAME_EXISTS_QUERY, parameters, cancellationToken);

        return await Task.FromResult(isExists);
    }
}
