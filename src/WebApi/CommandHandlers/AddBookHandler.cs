using Hangfire;
using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Extensions;
using Monday.WebApi.Jobs;

namespace Monday.WebApi.CommandHandlers;

public sealed class AddBookHandler : IRequestHandler<AddBook>
{
    private readonly IBackgroundJobClient jobClient;
    private readonly ILogger<AddBookHandler> logger;

    public AddBookHandler(IBackgroundJobClient jobClient, ILogger<AddBookHandler> logger)
    {
            this.jobClient = jobClient;
            this.logger = logger;
    }

    public Task Handle(AddBook request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "Book"),
            ("Name", request.Name)
        );

        this.logger.LogInformation("Queuing add book job");
        this.jobClient.Enqueue<AddBookJob>(job => job.Run(request.Name, request.ISBN, cancellationToken));
        this.logger.LogInformation("Add book job queued");

        return Task.CompletedTask;
    }
}