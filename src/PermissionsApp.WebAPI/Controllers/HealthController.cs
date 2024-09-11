using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PermissionsApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController(HealthCheckService healthCheckService) : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService = healthCheckService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();

            var result = new
            {
                status = healthReport.Status.ToString(),
                results = healthReport.Entries.Select(e => new
                {
                    key = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description
                })
            };

            return Ok(result);
        }
    }
}
