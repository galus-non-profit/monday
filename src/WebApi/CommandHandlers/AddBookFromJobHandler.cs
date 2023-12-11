using MediatR;
using Monday.WebApi.Commands;
using Monday.WebApi.Events;

namespace Monday.WebApi.CommandHandlers
{
    internal sealed class AddBookFromJobHandler : IRequestHandler<AddBookFromJob>
    {
        private readonly ILogger<AddBookFromJobHandler> logger;
        private readonly IMediator mediator;

        public AddBookFromJobHandler(ILogger<AddBookFromJobHandler> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Handle(AddBookFromJob request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Book created");

            var @event = new BookAdded { Name = request.Name };
            await mediator.Publish(@event, cancellationToken);
        }
    }
}
