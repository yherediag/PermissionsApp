using AutoMapper;
using MediatR;
using PermissionsApp.Application.DTOs;
using PermissionsApp.Application.Services;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Commands.ModifyPermission;

public record ModifyPermissionCommand(int PermissionId,
                                   string? EmployeeName,
                                   string? EmployeeSurname,
                                   int? PermissionTypeId) : IRequest;

public class ModifyPermissionHandler(IUnitOfWork uow,
                                     IMapper mapper,
                                     IElasticsearchService<ElasticPermission> elasticsearch) : IRequestHandler<ModifyPermissionCommand>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;
    private readonly IElasticsearchService<ElasticPermission> _elasticsearch = elasticsearch;

    public async Task Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _uow.Permissions.GetByIdAsync(request.PermissionId, cancellationToken)
            ?? throw new NotFoundException($"The permission '{request.PermissionId}' was not found");

        _mapper.Map(request, permission);

        permission.AddDomainEvent(new ModifyPermissionEvent(permission));

        _uow.Permissions.Update(permission);

        await _uow.SaveChangesAsync(cancellationToken);

        var elasticPermissionDto = new ElasticPermission(
            permission.PermissionId,
            permission.EmployeeName,
            permission.EmployeeSurname,
            permission.PermissionTypeId,
            permission.CreatedDate,
            permission.LastModifiedDate,
            permission.DeletedDate,
            permission.IsDeleted
        );
        await _elasticsearch.RequestOrModify(permission.PermissionId, elasticPermissionDto);
    }
}
