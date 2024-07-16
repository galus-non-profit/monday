namespace Monday.WebApi.Services;

using Monday.WebApi.Database.Interfaces;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Options;

internal class UserReadService : IUserReadService
{
    private const string GET_IS_USER_EXISTS = """
                                              SELECT
                                                [UserName]
                                                FROM
                                                    [dbo].[Users]
                                                WHERE
                                                    [UserName] like @Name
                                              """;

    private readonly IDBEngine dbEngine;
    private readonly SqlOptions sqlOptions;
    private readonly ILogger<UserReadService> logger;

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

        var isNotExists = await this.dbEngine.ExecuteAsync(this.sqlOptions.SqlConnectionString, GET_IS_USER_EXISTS, parameters, cancellationToken) is -1;

        return await Task.FromResult(isNotExists);
    }
}
