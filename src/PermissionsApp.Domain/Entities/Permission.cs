using PermissionsApp.Domain.Primitives;

namespace PermissionsApp.Domain.Entities;

public class Permission : IAuditableEntity, ISoftDelete
{
    public int PermissionId { get; set; }
    public required string EmployeeName { get; set; }
    public required string EmployeeSurname { get; set; }
    public required int PermissionTypeId { get; set; }
    public PermissionType PermissionType { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset LastModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
}
