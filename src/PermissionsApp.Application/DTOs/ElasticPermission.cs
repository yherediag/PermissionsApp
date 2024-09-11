namespace PermissionsApp.Application.DTOs;

public record ElasticPermission(int PermissionId,
                                   string EmployeeName,
                                   string EmployeeSurname,
                                   int PermissionTypeId,
                                   DateTimeOffset CreatedDate,
                                   DateTimeOffset LastModifiedDate,
                                   DateTimeOffset DeletedDate,
                                   bool IsDeleted);
