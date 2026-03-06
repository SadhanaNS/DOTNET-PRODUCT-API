using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogue.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    // GET /health
    // Used by Kubernetes liveness and readiness probes
    [HttpGet]
    public ActionResult GetHealth()
    {
        return Ok(new
        {
            status = "healthy",
            app = "Product Catalogue API",
            version = "1.0.0",
            timestamp = DateTime.UtcNow
        });
    }
}