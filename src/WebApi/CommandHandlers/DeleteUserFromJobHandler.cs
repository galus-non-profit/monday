namespace Monday.WebApi.CommandHandlers;

using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Domain;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.Interfaces;

internal sealed class DeleteUserFromJobHandler : IRequestHandler<DeleteUserFromJob>
{
    private readonly ILogger<DeleteUserFromJobHandler> logger;
    private readonly IMediator mediator;
    private readonly IUserRepository repository;

    public DeleteUserFromJobHandler(ILogger<DeleteUserFromJobHandler> logger, IMediator mediator, IUserRepository repository)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task Handle(DeleteUserFromJob request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Name", request.Id)
        );

        this.logger.LogInformation("Deleting user");

        this.logger.LogInformation("Deleting user to repository");

        var userIdToDelete = new UserId(request.Id);

        await this.repository.DeleteAsync(userIdToDelete, cancellationToken);
        this.logger.LogInformation("User has been deleted to repository");

        var @event = new UserDeleted()
        {
            Id = userIdToDelete.Value,
        };

        this.logger.LogInformation("Publishing information about the deleting of the user");
        await this.mediator.Publish(@event, cancellationToken);
        this.logger.LogInformation("Published information about the deleting of the user");
    }
}
