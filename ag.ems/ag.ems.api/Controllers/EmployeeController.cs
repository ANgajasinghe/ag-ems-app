using ag.ems.application.Common;
using ag.ems.application.Interfaces;
using ag.ems.domain.Const;
using ag.ems.domain.Entities;
using cube360.vbs.application.Common.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ag.ems.api.Controllers;

[Route("api/employee")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public EmployeeController(IIdentityService identityService)
    {
        _identityService = identityService;
    }


    [HttpGet]
    public async Task<ActionResult<List<UserProfile>>> Get()
    {
        return await _identityService.GetUsers();
    }
    
    [HttpPost]
    public async Task<ActionResult<Result>> Create([FromBody] UserProfile request)
    {
        request.Validate();
        var tokenResponse = await _identityService.InviteUserAsync(request.Email, 
            RoleConstant.Employee,
            request
        );
        return Ok(tokenResponse);
    }
    
    [HttpPost("lock")]
    public async Task<ActionResult> LockUser([FromBody] LockRequest request)
    {
        await _identityService.LockUserAsync(request.Email);
        return Ok();
    }

   
    
    
}