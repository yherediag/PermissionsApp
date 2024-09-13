using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Domain.Events;

public class ModifyPermissionEvent(Permission permission) : BaseEvent
{
    public Permission Permission { get; } = permission;
}
