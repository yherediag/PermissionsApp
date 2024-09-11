using AutoMapper;
using MediatR;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Queries.GetPermission;

public record GetPermissionQuery(int PermissionId) : IRequest<GetPermissionDto>;

public record GetPermissionDto(int PermissionId,
                                string? EmployeeName,
                                string? EmployeeSurname,
                                string? PermissionType,
                                DateTimeOffset Created);

public class GetPermissionHandler(IUnitOfWork uow,
                                      IMapper mapper) : IRequestHandler<GetPermissionQuery, GetPermissionDto>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<GetPermissionDto> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.PermissionId, cancellationToken)
            ?? throw new NotFoundException($"The permission '{request.PermissionId}' was not found");

        return _mapper.Map<GetPermissionDto>(permission);
    }
}