using AutoMapper;
using MediatR;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Commands.RequestPermission;

public record RequestPermissionCommand(string EmployeeName,
                                      string EmployeeSurname,
                                      int PermissionTypeId) : IRequest<int>;

public class RequestPermissionHandler(IUnitOfWork uow,
                                     IMapper mapper) : IRequestHandler<RequestPermissionCommand, int>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _uow = uow;

    public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = _mapper.Map<Permission>(request);

        await _uow.Permissions.AddAsync(permission, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return permission.PermissionId;
    }
}
