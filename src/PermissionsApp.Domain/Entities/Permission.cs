namespace PermissionsApp.Domain.Entities;

public class Permission
{
    public int PermissionId { get; set; }
    public required string EmployeeName { get; set; }
    public required string EmployeeSurname { get; set; }
    public DateTimeOffset AtCreated { get; set; }
    public required int PermissionTypeId { get; set; }
    public required PermissionType PermissionType { get; set; }
}
