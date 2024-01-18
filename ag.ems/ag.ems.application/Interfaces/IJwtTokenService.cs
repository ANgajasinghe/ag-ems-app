using ag.ems.domain.Entities.Identity;

namespace ag.ems.application.Interfaces;

public interface IJwtTokenService
{
    string GetAccessToken(
        string email,
        string userId,
        IList<string>? userRole);

    Task<string> GenerateRefreshTokenAsync(AppIdentityUser user);
    Task<bool> ValidateRefreshTokenAsync(AppIdentityUser user, string refreshToken);
}
