using ag.ems.application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ag.ems.api.Controllers;

[Route("api/dropdown")]
[ApiController]
[Authorize]
public class DropdownController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public DropdownController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [HttpGet("employee")]
    public async Task<ActionResult<List<string>>> GetEmployee()
    {
        var ret = await _identityService.GetUsersForDropDown();
        return Ok(ret);
    }
}