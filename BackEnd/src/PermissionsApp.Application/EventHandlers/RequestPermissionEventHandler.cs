using MediatR;
using Microsoft.Extensions.Logging;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Primitives;

namespace PermissionsApp.Application.EventHandlers;

public class RequestPermissionEventHandler(ILogger<RequestPermissionEventHandler> logger,
                                           IEventBus bus) : INotificationHandler<RequestPermissionEvent>
{
    private readonly ILogger<RequestPermissionEventHandler> _logger = logger;
    private readonly IEventBus _bus = bus;

    public async Task Handle(RequestPermissionEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("N5 Domain Event: {DomainEvent}", notification.GetType().Name);

        await _bus.Publish(new EventMessage(Operation.RequestPermission));
    }
}
