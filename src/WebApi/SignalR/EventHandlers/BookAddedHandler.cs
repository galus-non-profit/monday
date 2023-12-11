using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.SignalR.Hubs;

namespace Monday.WebApi.SignalR.EventHandlers;

internal sealed class BookAddedHandler : INotificationHandler<BookAdded>
{
    private readonly IHubContext<BookHub> hubContext;

    public BookAddedHandler(IHubContext<BookHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public async Task Handle(BookAdded notification, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync("bookAdded", notification.Name, cancellationToken);
    }
}