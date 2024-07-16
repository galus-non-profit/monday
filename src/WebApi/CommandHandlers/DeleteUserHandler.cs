namespace Monday.WebApi.CommandHandlers;

using Hangfire;
using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Extensions;
using Monday.WebApi.Jobs;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUser>
{
    private readonly IBackgroundJobClient jobClient;
    private readonly ILogger<DeleteUserHandler> logger;

    public DeleteUserHandler(IBackgroundJobClient jobClient, ILogger<DeleteUserHandler> logger)
    {
        this.jobClient = jobClient;
        this.logger = logger;
    }

    public Task Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Id", request.Id)
        );

        this.logger.LogInformation("Queuing add book job");
        this.jobClient.Enqueue<DeleteUserJob>(job => job.Run(request.Id, cancellationToken));
        this.logger.LogInformation("Add book job queued");

        return Task.CompletedTask;
    }
}
