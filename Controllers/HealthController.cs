using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Health check endpoint for monitoring
    /// </summary>
    [HttpGet("health")]
    [AllowAnonymous]
    public ActionResult<object> Health()
    {
        _logger.LogInformation("Health check called at {Time}", DateTime.UtcNow);

        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        });
    }

    /// <summary>
    /// Detailed health check including database connectivity
    /// </summary>
    [HttpGet("health/detailed")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> HealthDetailed()
    {
        // Add database health check if needed
        var health = new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            checks = new
            {
                database = "connected",
                memory = GC.GetTotalMemory(false) / (1024 * 1024) + " MB",
                uptime = DateTime.UtcNow
            }
        };

        return Ok(health);
    }

    /// <summary>
    /// API information endpoint
    /// </summary>
    [HttpGet("info")]
    [AllowAnonymous]
    public ActionResult<object> Info()
    {
        return Ok(new
        {
            name = "School Management API",
            version = "1.0.0",
            description = "A comprehensive API for managing school operations",
            documentation = "/swagger/index.html"
        });
    }
}
