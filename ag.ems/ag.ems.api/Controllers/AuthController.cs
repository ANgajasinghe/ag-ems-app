using ag.ems.application.Interfaces;
using cube360.vbs.application.Common.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ag.ems.api.Controllers;

[Route("api/auth")]
[ApiController]
[Authorize]
public class LoginController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public LoginController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var tokenResponse = await _identityService.LoginAsync(request.Email, request.Password);
        return Ok(tokenResponse);
    }
    
    [HttpPost("lock")]
    public async Task<ActionResult> LockUser([FromBody] LoginRequest request)
    {
        await _identityService.LockUserAsync(request.Email);
        return Ok();
    }
    
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _identityService.ChangePasswordAsync( request.OldPassword, request.NewPassword);
        return Ok();
    }
    
    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] UserProfile request)
    {
        await _identityService.UpdateMyProfileAsync( request);
        return Ok();
    }
    
    
}