using MediatR;
using Microsoft.AspNetCore.Mvc;
using PermissionsApp.Application.Commands.ModifyPermission;
using PermissionsApp.Application.Commands.RemovePermission;
using PermissionsApp.Application.Commands.RequestPermission;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Application.Queries.GetPermissions;
using PermissionsApp.Domain.Exceptions;

namespace PermissionsApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(ISender mediator) : ControllerBase
    {
        private readonly ISender _mediator = mediator;

        [HttpGet(Name = nameof(GetPermissions))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPermissionsResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPermissions([FromQuery] GetPermissionsQuery query)
        {
            var permissionsDto = await _mediator.Send(query);

            return Ok(permissionsDto);
        }

        [HttpGet("{permissionId}", Name = nameof(GetPermission))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPermissionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPermission(int permissionId)
        {
            var permissionDto = await _mediator.Send(new GetPermissionQuery(permissionId));

            return Ok(permissionDto);
        }

        [HttpPost(Name = nameof(RequestPermission))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
        {
            var permissionId = await _mediator.Send(command);

            return CreatedAtRoute(nameof(GetPermission), new { permissionId }, new { permissionId });
        }

        [HttpPut("{permissionId}", Name = nameof(ModifyPermissions))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModifyPermissions(int permissionId, [FromBody] ModifyPermissionCommand command)
        {
            if (permissionId != command.PermissionId)
                throw new BadRequestException($"The permissionId '{permissionId}' is different than the body permissionId.");

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{permissionId}", Name = nameof(RemovePermissions))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemovePermissions(int permissionId)
        {
            await _mediator.Send(new RemovePermissionCommand(permissionId));

            return NoContent();
        }
    }
}
