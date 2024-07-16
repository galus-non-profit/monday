namespace Monday.WebApi.CommandHandlers;

using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Domain;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.Interfaces;

internal sealed class AddBookFromJobHandler : IRequestHandler<AddBookFromJob>
{
    private readonly ILogger<AddBookFromJobHandler> logger;
    private readonly IMediator mediator;
    private readonly IBookRepository repository;

    public AddBookFromJobHandler(ILogger<AddBookFromJobHandler> logger, IMediator mediator, IBookRepository repository)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task Handle(AddBookFromJob request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "Book"),
            ("Name", request.Name)
        );

        this.logger.LogInformation("Creating book");

        var entity = new BookEntity
        {
            Name = request.Name,
        };

        this.logger.LogInformation("Adding book to repository");
        await this.repository.CreateAsync(entity, cancellationToken);
        this.logger.LogInformation("Book has been added to repository");

        var @event = new BookAdded
        {
            Name = entity.Name,
            ISBN = entity.ISBN,
        };

        this.logger.LogInformation("Publishing information about the creation of the book");
        await this.mediator.Publish(@event, cancellationToken);
        this.logger.LogInformation("Published information about the creation of the book");
    }
}
