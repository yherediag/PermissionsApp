using AutoMapper;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Application.Queries.GetPermissions;

public class GetPermissionsMapping : Profile
{
    public GetPermissionsMapping()
    {
        CreateMap<IEnumerable<Permission>, IEnumerable<GetPermissionDto>>().ConvertUsing(new GetPermissionsTypeConverter());
    }

    private class GetPermissionsTypeConverter : ITypeConverter<IEnumerable<Permission>, IEnumerable<GetPermissionDto>>
    {
        public IEnumerable<GetPermissionDto> Convert(IEnumerable<Permission> source, IEnumerable<GetPermissionDto> destination, ResolutionContext context)
        {
            return source
                .Select(context.Mapper.Map<Permission, GetPermissionDto>);
        }
    }
}