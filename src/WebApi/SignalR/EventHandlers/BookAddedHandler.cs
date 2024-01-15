using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

namespace Monday.WebApi.SignalR.EventHandlers;

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

        logger.LogInformation("Publishing book added event on hub: {Status}", status.Status );
        await hubContext.Clients.All.SendAsync("AddBookStatus", status, cancellationToken);
        logger.LogInformation("Published book added event on hub: {Status}", status.Status );
    }
}