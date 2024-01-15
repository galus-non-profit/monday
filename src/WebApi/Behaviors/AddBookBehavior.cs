using MediatR;
using Monday.WebApi.Events;
using Monday.WebApi.Interfaces;

namespace Monday.WebApi.Behaviors
{
    public sealed class AddBookBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IAddBook
    {
        private readonly IPublisher mediator;

        public AddBookBehavior(IPublisher mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch
            {
                var @event = new BookNotAdded
                {
                    ISBN = request.ISBN,
                    Name = request.Name,
                };

                await mediator.Publish(@event, cancellationToken);
                throw;
            }
        }
    }
}
