namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class BookAddedHandler : INotificationHandler<BookAdded>
{
    private readonly IHubContext<BookHub> hubContext;
    private readonly ILogger<BookAddedHandler> logger;

    public BookAddedHandler(IHubContext<BookHub> hubContext, ILogger<BookAddedHandler> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(BookAdded notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "Book"),
            ("Name", notification.Name)
        );

        var status = new AddBookStatus
        {
            ISBN = notification.ISBN,
            Name = notification.Name,
            Status = "Succeed",
        };

        this.logger.LogInformation("Publishing book added event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("AddBookStatus", status, cancellationToken);
        this.logger.LogInformation("Published book added event on hub: {Status}", status.Status);
    }
}
