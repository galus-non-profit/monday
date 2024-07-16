namespace Monday.WebApi.Services;

using Monday.WebApi.Database.Interfaces;
using Monday.WebApi.Domain;
using Monday.WebApi.Extensions;
using Monday.WebApi.Interfaces;
using Monday.WebApi.Options;

public sealed class UserRepository : IUserRepository
{
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
                                                       ,@PasswordHashed)
                                                 """;

    private const string DELETE_USER_COMMAND = """
                                                DELETE FROM [dbo].[Users]
                                                    WHERE [Id] = @UserGuid
                                               """;
    private readonly IDBEngine dbEngine;
    private readonly SqlOptions sqlOptions;
    private readonly ILogger<UserRepository> logger;

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

        var userModel = entity.ToDbEnity();

        var parameters = new
        {
            UserGuid = userModel.UserId,
            UserName = userModel.Name,
            Email = userModel.Email,
            Password = userModel.PasswordHash,
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
}
