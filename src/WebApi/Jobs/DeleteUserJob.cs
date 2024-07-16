namespace Monday.WebApi.Jobs;

using System.ComponentModel;
using MediatR;
using Monday.WebApi.Commands;

public sealed class DeleteUserJob
{
    private readonly IMediator mediator;

    public DeleteUserJob(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [DisplayName("Delete user job")]
    public async Task Run(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserFromJob()
        {
            Id = id,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
