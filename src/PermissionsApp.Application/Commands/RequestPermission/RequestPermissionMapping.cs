using AutoMapper;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Application.Commands.RequestPermission;

internal class RequestPermissionMapping : Profile
{
    public RequestPermissionMapping()
    {
        CreateMap<RequestPermissionCommand, Permission>().ConvertUsing(new RequestPermissionTypeConverter());
    }

    private class RequestPermissionTypeConverter : ITypeConverter<RequestPermissionCommand, Permission>
    {
        public Permission Convert(RequestPermissionCommand source, Permission destination, ResolutionContext context)
        {
            return new Permission
            {
                EmployeeName = source.EmployeeName,
                EmployeeSurname = source.EmployeeSurname,
                PermissionTypeId = source.PermissionTypeId
            };
        }
    }
}
