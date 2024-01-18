using System.Security.Claims;
using ag.ems.application.Interfaces;
using Microsoft.AspNetCore.Http;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private string _userId = string.Empty;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? _userId ?? string.Empty;

    public void SetUserId(string userId)
    {
        _userId = userId;
    }

    public List<string> UserRoles() => _httpContextAccessor.HttpContext?.User?.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(x => x.Value)
        .ToList() ?? new List<string>();

    public string UserEmail => _httpContextAccessor.HttpContext?.User?.Claims?
        .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
    

    public string BranchId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("branchId") ?? string.Empty;
}