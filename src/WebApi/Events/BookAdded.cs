﻿using MediatR;

namespace Monday.WebApi.Events;

public sealed record class BookAdded : INotification
{
    public required string ISBN { get; init; }
    public required string Name { get; init; }
}