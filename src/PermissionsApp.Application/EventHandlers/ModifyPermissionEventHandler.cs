using MediatR;
using Microsoft.Extensions.Logging;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Primitives;

namespace PermissionsApp.Application.EventHandlers;

public class ModifyPermissionEventHandler(ILogger<ModifyPermissionEventHandler> logger,
                                            IEventBus bus) : INotificationHandler<ModifyPermissionEvent>
{
    private readonly ILogger<ModifyPermissionEventHandler> _logger = logger;
    private readonly IEventBus _bus = bus;

    public async Task Handle(ModifyPermissionEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("N5 Domain Event: {DomainEvent}", notification.GetType().Name);

        await _bus.Publish(new EventMessage(Operation.ModifyPermission));
    }
}
