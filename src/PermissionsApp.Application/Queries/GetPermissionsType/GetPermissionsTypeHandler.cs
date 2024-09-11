using AutoMapper;
using MediatR;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.Application.Queries.GetPermissionsType;

public record GetPermissionsTypeQuery : IRequest<IEnumerable<GetPermissionTypeDto>>;

public record GetPermissionTypeDto(int PermissionTypeId,
                                   string Description);

public class GetPermissionsTypeHandler(IUnitOfWork uow,
                                       IMapper mapper) : IRequestHandler<GetPermissionsTypeQuery, IEnumerable<GetPermissionTypeDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GetPermissionTypeDto>> Handle(GetPermissionsTypeQuery request, CancellationToken cancellationToken)
    {
        var permissionsType = await _uow.PermissionsType.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<GetPermissionTypeDto>>(permissionsType);
    }
}
