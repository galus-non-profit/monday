namespace Monday.WebApi.Services;

using Monday.WebApi.Database.Interfaces;
using Monday.WebApi.Domain;
using Monday.WebApi.Extensions;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Options;
using Monday.WebApi.Results;

public sealed class UserRepository : IUserRepository
{
    private const string DELETE_USER_COMMAND = """
                                                DELETE FROM [dbo].[Users]
                                                    WHERE [Id] = @UserGuid
                                               """;

    private const string GET_USERS_COMMAND = """
                                              SELECT
                                                 [Id]
                                                ,[UserName]
                                                ,[Email]
                                                ,[PasswordHash]
                                                FROM [dbo].[Users]
                                             """;

    private const string INSERT_USER_COMMAND = """
                                               INSERT INTO [dbo].[Users]
                                                     ([Id]
                                                     ,[UserName]
                                                     ,[Email]
                                                     ,[PasswordHash])
                                               VALUES
                                                     (@UserGuid
                                                     ,@UserName
                                                     ,@Email
                                                     ,@PasswordHash)
                                               """;

    private readonly IDBEngine dbEngine;
    private readonly ILogger<UserRepository> logger;
    private readonly SqlOptions sqlOptions;

    public UserRepository(IDBEngine dbEngine, SqlOptions sqlOptions, ILogger<UserRepository> logger)
    {
        this.dbEngine = dbEngine;
        this.sqlOptions = sqlOptions;
        this.logger = logger;
    }

    public async Task CreateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            (nameof(UserId), entity.Id.Value));

        this.logger.LogInformation("Try to create user in database");

        var userModel = entity.ToDbEntity();

        var parameters = new
        {
            UserGuid = userModel.Id,
            UserName = userModel.UserName,
            Email = userModel.Email,
            PasswordHash = userModel.PasswordHash,
        };

        await this.dbEngine.ExecuteAsync(this.sqlOptions.SqlConnectionString, INSERT_USER_COMMAND, parameters, cancellationToken);
    }

    public async Task DeleteAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            (nameof(UserId), userId.Value));

        this.logger.LogInformation("Try to delete user from database");

        var parameters = new
        {
            UserGuid = userId.Value,
        };

        await this.dbEngine.ExecuteAsync(this.sqlOptions.SqlConnectionString, DELETE_USER_COMMAND, parameters, cancellationToken);
    }

    public async Task<IReadOnlyList<UserResult>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Try to delete user from database");

        var result = await this.dbEngine.QueryAsync<UserDBEntity>(this.sqlOptions.SqlConnectionString, GET_USERS_COMMAND, parameters: null, cancellationToken);

        var userResults = result.Select(x => new UserResult()
        {
            Id = x.Id,
            Email = x.Email,
            Name = x.UserName,
            Password = x.PasswordHash,
        });

        return userResults.ToList().AsReadOnly();
    }
}
