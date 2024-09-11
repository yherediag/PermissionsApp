using MediatR;
using Microsoft.Extensions.Logging;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Primitives;

namespace PermissionsApp.Application.EventHandlers;

public class RemovePermissionEventHandler(ILogger<RemovePermissionEventHandler> logger,
                                            IEventBus bus) : INotificationHandler<RemovePermissionEvent>
{
    private readonly ILogger<RemovePermissionEventHandler> _logger = logger;
    private readonly IEventBus _bus = bus;

    public async Task Handle(RemovePermissionEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("N5 Domain Event: {DomainEvent}", notification.GetType().Name);

        await _bus.Publish(new EventMessage(Operation.RemovePermission));
    }
}
