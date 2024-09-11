using MediatR;
using Microsoft.AspNetCore.Mvc;
using PermissionsApp.Application.Queries.GetPermissionsType;

namespace PermissionsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsTypeController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet(Name = nameof(GetPermissionsType))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPermissionTypeDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPermissionsType()
        {
            var permissionsDto = await _mediator.Send(new GetPermissionsTypeQuery());

            return Ok(permissionsDto);
        }
    }
}
