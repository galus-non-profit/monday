using System.ComponentModel;
using Hangfire;
using MediatR;
using Monday.WebApi.Commands;

namespace Monday.WebApi.Jobs;

public sealed class AddBookJob
{
    private readonly IMediator mediator;
    public AddBookJob(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [DisplayName("Add book example job")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run(string name, CancellationToken cancellationToken = default)
    {
        var command = new AddBookFromJob { Name = name };
        await this.mediator.Send(command, cancellationToken);
    }
}