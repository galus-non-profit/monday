namespace Monday.WebApi.Jobs;

using System.ComponentModel;
using Hangfire;
using Monday.WebApi.Commands;

internal sealed class AddUserJob
{
    private readonly IMediator mediator;

    public AddUserJob(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [DisplayName("Add user job"), AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid id, string email, string name, string passwordHash, CancellationToken cancellationToken = default)
    {
        var command = new AddUserFromJob
        {
            Id = id,
            Email = email,
            Name = name,
            Password = passwordHash,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
