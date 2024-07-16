namespace Monday.WebApi.Behaviors;

using MediatR;
using Monday.WebApi.Events;
using Monday.WebApi.Interfaces;

public sealed class AddUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IAddUser
{
    private readonly IPublisher mediator;

    public AddUserBehavior(IPublisher mediator)
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
            var @event = new UserNotAdded()
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
            };

            await this.mediator.Publish(@event, cancellationToken);

            throw;
        }
    }
}