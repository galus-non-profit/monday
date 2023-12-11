using Hangfire;
using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Jobs;

namespace Monday.WebApi.CommandHandlers;

public sealed class AddBookHandler : IRequestHandler<AddBook>
{
    private readonly IBackgroundJobClient jobClient;

    public AddBookHandler(IBackgroundJobClient jobClient)
    {
            this.jobClient = jobClient;
    }

    public Task Handle(AddBook request, CancellationToken cancellationToken)
    {
        this.jobClient.Enqueue<AddBookJob>(i => i.Run(request.Name, cancellationToken));

        return Task.CompletedTask;
    }
}