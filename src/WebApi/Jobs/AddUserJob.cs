namespace Monday.WebApi.Jobs;

using System.ComponentModel;
using Hangfire;
using MediatR;
using Monday.WebApi.Commands;

public sealed class AddUserJob
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
            PasswordHashed = passwordHash,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}