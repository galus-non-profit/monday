namespace Monday.WebApi.CommandHandlers;

using Hangfire;
using MediatR;
using Monday.WebApi.Auth;
using Monday.WebApi.Commands;
using Monday.WebApi.Extensions;
using Monday.WebApi.Jobs;

public sealed class AddUserHandler : IRequestHandler<AddUser>
{
    private readonly IBackgroundJobClient jobClient;
    private readonly IPasswordHasher passwordHasher;
    private readonly ILogger<AddBookHandler> logger;

    public AddUserHandler(IBackgroundJobClient jobClient,IPasswordHasher passwordHasher, ILogger<AddBookHandler> logger)
    {
        this.jobClient = jobClient;
        this.passwordHasher = passwordHasher;
        this.logger = logger;
    }

    public Task Handle(AddUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Name", request.Name)
        );

        var hashedPassword = this.passwordHasher.Hash(request.Password);

        this.logger.LogInformation("Queuing add book job");
        this.jobClient.Enqueue<AddUserJob>(job => job.Run(request.Id, request.Email, request.Name, hashedPassword, cancellationToken));
        this.logger.LogInformation("Add book job queued");

        return Task.CompletedTask;
    }
}