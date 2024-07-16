namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class UserNotAddedHandler : INotificationHandler<UserNotAdded>
{
    private readonly IHubContext<UserHub> hubContext;
    private readonly ILogger<UserNotAdded> logger;

    public UserNotAddedHandler(IHubContext<UserHub> hubContext, ILogger<UserNotAdded> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(UserNotAdded notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Id", notification.Id)
        );

        var status = new AddUserStatus()
        {
            Name = notification.Name,
            Status = "Failed",
            Email = notification.Email,
            Id = notification.Id,
        };

        this.logger.LogInformation("Publishing user not added event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("AddUserStatus", status, cancellationToken);
        this.logger.LogInformation("Published user not added event on hub: {Status}", status.Status);
    }
}