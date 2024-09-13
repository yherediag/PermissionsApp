using AutoMapper;
using MediatR;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Queries.GetPermissions;

public record GetPermissionsQuery(int PageNumber = 1, int PageSize = 5) : IRequest<GetPermissionsResponse>;

public record GetPermissionsResponse(IEnumerable<GetPermissionDto> Permissions, int TotalCount);

public class GetPermissionsHandler(IUnitOfWork uow,
                                   IMapper mapper,
                                   IEventBus bus) : IRequestHandler<GetPermissionsQuery, GetPermissionsResponse>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;
    private readonly IEventBus _bus = bus;

    public async Task<GetPermissionsResponse> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissionsEntity = await _uow.Permissions.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        await _bus.Publish(new EventMessage(Operation.GetPermissions));

        var permissionsDto = _mapper.Map<IEnumerable<GetPermissionDto>>(permissionsEntity.Permissions);
        return new GetPermissionsResponse(permissionsDto, permissionsEntity.TotalCount);
    }
}
