namespace Monday.WebApi.Behaviors;

using MediatR;
using Monday.WebApi.Events;
using Monday.WebApi.Interfaces;

public sealed class DeleteUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IDeleteUser
{
    private readonly IPublisher mediator;

    public DeleteUserBehavior(IPublisher mediator)
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
            var failureEvent = new UserNotDeleted()
            {
                Id = request.Id,
            };

            await this.mediator.Publish(failureEvent, cancellationToken);

            throw;
        }
    }
}
