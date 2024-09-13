namespace PermissionsApp.Domain.Entities;

public class PermissionType
{
    public int PermissionTypeId { get; set; }
    public required string Description { get; set; }

    public ICollection<Permission> Permissions { get; set; }
}
