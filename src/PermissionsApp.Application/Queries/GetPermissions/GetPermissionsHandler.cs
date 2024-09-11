﻿using AutoMapper;
using MediatR;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Queries.GetPermissions;

public record GetPermissionsQuery : IRequest<IEnumerable<GetPermissionDto>>;

public class GetPermissionsHandler(IUnitOfWork uow,
                                   IMapper mapper,
                                   IEventBus bus) : IRequestHandler<GetPermissionsQuery, IEnumerable<GetPermissionDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;
    private readonly IEventBus _bus = bus;

    public async Task<IEnumerable<GetPermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _uow.Permissions.GetAllAsync(cancellationToken);

        await _bus.Publish(new EventMessage(Operation.GetPermissions));

        return _mapper.Map<IEnumerable<GetPermissionDto>>(permissions);
    }
}
