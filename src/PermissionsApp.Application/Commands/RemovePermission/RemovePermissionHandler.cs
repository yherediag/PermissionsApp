using MediatR;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Commands.RemovePermission;

public record RemovePermissionCommand(int PermissionId) : IRequest<bool>;

public class RemovePermissionHandler(IUnitOfWork uow) : IRequestHandler<RemovePermissionCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<bool> Handle(RemovePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.PermissionId, cancellationToken)
            ?? throw new NotFoundException($"The permission '{request.PermissionId}' was not found");

        permission.AddDomainEvent(new RemovePermissionEvent(permission));

        _uow.Permissions.Delete(permission);

        await _uow.SaveChangesAsync(cancellationToken);

        return true;
    }
}