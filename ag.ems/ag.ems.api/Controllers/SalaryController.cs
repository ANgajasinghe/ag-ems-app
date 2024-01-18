using ag.ems.application.Common;
using ag.ems.application.Services;
using cube360.vbs.application.Common.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ag.ems.api.Controllers;

[Route("api/salary")]
[ApiController]
[Authorize]
public class SalaryController : ControllerBase
{
    private readonly SalaryService _salaryService;


    public SalaryController(SalaryService salaryService)
    {
        _salaryService = salaryService;
    }
    

    [HttpPost]
    public async Task<ActionResult<Result>> Create([FromBody] SalaryRequest request)
    {

        var salary = await _salaryService.CreateSalaryAsync(request);
        return Ok(salary);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<SalaryResponse>>> Get(string? email, string? month, CancellationToken cancellationToken)
    {
        var salary = await _salaryService.GetSalaryAsync(email, month, cancellationToken);
        return Ok(salary);
    }
}