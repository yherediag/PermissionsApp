using AutoMapper;
using MediatR;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Commands.ModifyPermission;

public record ModifyPermissionCommand(int PermissionId,
                                   string? EmployeeName,
                                   string? EmployeeSurname,
                                   int? PermissionTypeId) : IRequest;

public class ModifyPermissionHandler(IUnitOfWork uow,
                                     IMapper mapper) : IRequestHandler<ModifyPermissionCommand>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.PermissionId, cancellationToken)
            ?? throw new NotFoundException($"The permission '{request.PermissionId}' was not found");

        _mapper.Map(request, permission);

        permission.AddDomainEvent(new ModifyPermissionEvent(permission));

        _uow.Permissions.Update(permission);

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
