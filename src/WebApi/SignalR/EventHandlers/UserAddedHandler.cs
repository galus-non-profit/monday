namespace Monday.WebApi.SignalR.EventHandlers;

using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monday.WebApi.Events;
using Monday.WebApi.Extensions;
using Monday.WebApi.SignalR.Hubs;
using Monday.WebApi.SignalR.Models;

internal sealed class UserAddedHandler : INotificationHandler<UserAdded>
{
    private readonly IHubContext<UserHub> hubContext;
    private readonly ILogger<UserAddedHandler> logger;

    public UserAddedHandler(IHubContext<UserHub> hubContext, ILogger<UserAddedHandler> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }

    public async Task Handle(UserAdded notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Scope", "User"),
            ("Id", notification.Id)
        );

        var status = new AddUserStatus()
        {
            Id = notification.Id,
            Name = notification.Name,
            Email = notification.Email,
            Status = "Succeed",
        };

        this.logger.LogInformation("Publishing user added event on hub: {Status}", status.Status);
        await this.hubContext.Clients.All.SendAsync("AddUserStatus", status, cancellationToken);
        this.logger.LogInformation("Published user added event on hub: {Status}", status.Status);
    }
}