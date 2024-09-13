using AutoMapper;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Application.Queries.GetPermission;

public class GetPermissionMapping : Profile
{
    public GetPermissionMapping()
    {
        CreateMap<Permission, GetPermissionDto>().ConvertUsing(new GetPermissionTypeConverter());
    }

    private class GetPermissionTypeConverter : ITypeConverter<Permission, GetPermissionDto>
    {
        public GetPermissionDto Convert(Permission source, GetPermissionDto destination, ResolutionContext context)
        {
            return new(source.PermissionId,
                       source.EmployeeName,
                       source.EmployeeSurname,
                       source.PermissionType.PermissionTypeId,
                       source.PermissionType.Description,
                       source.CreatedDate,
                       source.LastModifiedDate);
        }
    }
}