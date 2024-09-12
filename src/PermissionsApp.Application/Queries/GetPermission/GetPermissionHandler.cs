using AutoMapper;
using MediatR;
using PermissionsApp.Domain.Enums;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Queries.GetPermission;

public record GetPermissionQuery(int PermissionId) : IRequest<GetPermissionDto>;

public record GetPermissionDto(int PermissionId,
                                string? EmployeeName,
                                string? EmployeeSurname,
                                int? PermissionTypeId,
                                string? PermissionTypeDescription,
                                DateTimeOffset Created,
                                DateTimeOffset LastModified);

public class GetPermissionHandler(IUnitOfWork uow,
                                  IMapper mapper,
                                  IEventBus bus) : IRequestHandler<GetPermissionQuery, GetPermissionDto>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;
    private readonly IEventBus _bus = bus;

    public async Task<GetPermissionDto> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.PermissionId, cancellationToken)
            ?? throw new NotFoundException($"The permission '{request.PermissionId}' was not found");

        await _bus.Publish(new EventMessage(Operation.GetPermission));

        return _mapper.Map<GetPermissionDto>(permission);
    }
}