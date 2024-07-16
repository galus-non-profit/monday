namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class UserDeletedHandler : INotificationHandler<UserDeleted>
{
    private readonly IHubContext<UserHub> hubContext;
    private readonly ILogger<UserDeletedHandler> logger;

    public UserDeletedHandler(IHubContext<UserHub> hubContext, ILogger<UserDeletedHandler> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(UserDeleted notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Id", notification.Id)
        );

        var status = new DeleteUserStatus()
        {
            Id = notification.Id,
            Status = "Succeed",
        };

        this.logger.LogInformation("Publishing user delete event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("DeleteUserStatus", status, cancellationToken);
        this.logger.LogInformation("Published user delete event on hub: {Status}", status.Status);
    }
}
