using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ag.ems.api.Controllers;

[Route("api/data-seeder")]
[ApiController]
[Authorize]
public class DataSeederController : ControllerBase
{
    private readonly DataSeeder _dataSeeder;

    public DataSeederController(DataSeeder dataSeeder)
    {
        _dataSeeder = dataSeeder;
    }
    
    [HttpPost]
    public async Task<ActionResult> Seed(CancellationToken stoppingToken)
    {
        await _dataSeeder.ExecuteAsync(stoppingToken);
        return Ok();
    }
    
}