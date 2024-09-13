using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Domain.Events;

public class RequestPermissionEvent(Permission permission) : BaseEvent
{
    public Permission Permission { get; } = permission;
}
