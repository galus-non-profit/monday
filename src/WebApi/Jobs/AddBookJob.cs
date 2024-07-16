namespace Monday.WebApi.Jobs;

using System.ComponentModel;
using Hangfire;
using MediatR;
using Monday.WebApi.Commands;

public sealed class AddBookJob
{
    private readonly IMediator mediator;

    public AddBookJob(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [DisplayName("Add book example job"), AutomaticRetry(Attempts = 3)]
    public async Task Run(string name, string isbn, CancellationToken cancellationToken = default)
    {
        var command = new AddBookFromJob
        {
            Name = name,
            ISBN = isbn,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
