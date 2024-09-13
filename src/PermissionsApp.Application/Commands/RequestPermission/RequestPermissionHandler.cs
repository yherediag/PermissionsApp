using AutoMapper;
using MediatR;
using PermissionsApp.Application.DTOs;
using PermissionsApp.Application.Services;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Commands.RequestPermission;

public record RequestPermissionCommand(string EmployeeName,
                                      string EmployeeSurname,
                                      int PermissionTypeId) : IRequest<int>;

public class RequestPermissionHandler(IUnitOfWork uow,
                                      IMapper mapper,
                                      IElasticsearchService<ElasticPermission> elasticsearch) : IRequestHandler<RequestPermissionCommand, int>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _uow = uow;
    private readonly IElasticsearchService<ElasticPermission> _elasticsearch = elasticsearch;

    public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = _mapper.Map<Permission>(request);

        permission.AddDomainEvent(new RequestPermissionEvent(permission));

        await _uow.Permissions.AddAsync(permission, cancellationToken);
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

        return permission.PermissionId;
    }
}
