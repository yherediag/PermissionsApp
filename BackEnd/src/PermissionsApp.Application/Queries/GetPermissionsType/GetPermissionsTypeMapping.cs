using AutoMapper;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Application.Queries.GetPermissionsType;

public class GetPermissionsTypeMapping : Profile
{
    public GetPermissionsTypeMapping()
    {
        CreateMap<IEnumerable<PermissionType>, IEnumerable<GetPermissionTypeDto>>().ConvertUsing(new GetPermissionsTypeTypeConverter());
    }

    private class GetPermissionsTypeTypeConverter : ITypeConverter<IEnumerable<PermissionType>, IEnumerable<GetPermissionTypeDto>>
    {
        public IEnumerable<GetPermissionTypeDto> Convert(IEnumerable<PermissionType> source, IEnumerable<GetPermissionTypeDto> destination, ResolutionContext context)
        {
            return source
                .Select(type => new GetPermissionTypeDto(type.PermissionTypeId,
                                                         type.Description));
        }
    }
}
