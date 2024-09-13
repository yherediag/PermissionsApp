using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Domain.Events;

public class RemovePermissionEvent(Permission permission) : BaseEvent
{
    public Permission Permission { get; } = permission;
}
