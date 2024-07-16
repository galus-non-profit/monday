namespace Monday.WebApi.CommandHandlers;

using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Domain;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.Interfaces;

internal sealed class AddUserFromJobHandler : IRequestHandler<AddUserFromJob>
{
    private readonly ILogger<AddUserFromJobHandler> logger;
    private readonly IMediator mediator;
    private readonly IUserRepository repository;

    public AddUserFromJobHandler(ILogger<AddUserFromJobHandler> logger, IMediator mediator, IUserRepository repository)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task Handle(AddUserFromJob request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Name", request.Name)
        );

        this.logger.LogInformation("Creating user");

        var entity = new UserEntity(
            request.Name,
            new Email(request.Email),
            new UserId(request.Id),
            new PasswordHashed(request.PasswordHashed));

        this.logger.LogInformation("Adding user to repository");
        await this.repository.CreateAsync(entity, cancellationToken);
        this.logger.LogInformation("User has been added to repository");

        var @event = new UserAdded()
        {
            Name = entity.Name,
            Id = entity.Id.Value,
            Email = entity.Email.Value,
        };

        this.logger.LogInformation("Publishing information about the creation of the user");
        await this.mediator.Publish(@event, cancellationToken);
        this.logger.LogInformation("Published information about the creation of the user");
    }
}
