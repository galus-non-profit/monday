namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class BookNotAddedHandler : INotificationHandler<BookNotAdded>
{
    private readonly IHubContext<BookHub> hubContext;
    private readonly ILogger<BookNotAdded> logger;

    public BookNotAddedHandler(IHubContext<BookHub> hubContext, ILogger<BookNotAdded> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(BookNotAdded notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "Book"),
            ("Name", notification.Name)
        );

        var status = new AddBookStatus
        {
            Name = notification.Name,
            Status = "Failed",
            ISBN = notification.ISBN,
        };

        this.logger.LogInformation("Publishing book not added event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("AddBookStatus", status, cancellationToken);
        this.logger.LogInformation("Published book not added event on hub: {Status}", status.Status);
    }
}
