namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class UserNotDeletedHandler : INotificationHandler<UserNotDeleted>
{
    private readonly IHubContext<UserHub> hubContext;
    private readonly ILogger<UserNotDeletedHandler> logger;

    public UserNotDeletedHandler(IHubContext<UserHub> hubContext, ILogger<UserNotDeletedHandler> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(UserNotDeleted notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Id", notification.Id)
        );

        var status = new DeleteUserStatus()
        {
            Id = notification.Id,
            Status = "Failed",
        };

        this.logger.LogInformation("Publishing user not deleted event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("DeleteUserStatus", status, cancellationToken);
        this.logger.LogInformation("Published user not deleted event on hub: {Status}", status.Status);
    }
}
