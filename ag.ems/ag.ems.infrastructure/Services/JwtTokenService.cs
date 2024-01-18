using System.IdentityModel.Tokens.Jwt;
using ag.ems.application.Common;
using ag.ems.application.Interfaces;
using ag.ems.domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Throw;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ag.ems.infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly ApplicationConfig _applicationConfig;
        private readonly UserManager<AppIdentityUser> _userManager;

        public JwtTokenService(UserManager<AppIdentityUser> userManager, ApplicationConfig applicationConfig)
        {
            _userManager = userManager;
            _applicationConfig = applicationConfig;
        }

        public string GetAccessToken(
            string email,
            string userId,
            IList<string>? userRole)
        {
            var claims = GetAccessTokenClaims(email, userId, userRole);
            return GenerateAccessToken(claims);
        }



        public async Task<string> GenerateRefreshTokenAsync(AppIdentityUser user)
        {
            var removeTokenResult =
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

            removeTokenResult.Throw(string.Join(',', removeTokenResult.Errors.Select(e => e.Description).ToArray()))
                .IfFalse(x => x.Succeeded);

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");

            var tokenStoreResults =
                await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken",
                    newRefreshToken);

            tokenStoreResults.Throw(string.Join(',', tokenStoreResults.Errors.Select(e => e.Description).ToArray()))
                .IfFalse(x => x.Succeeded);

            return newRefreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(AppIdentityUser user, string refreshToken)
        {
            return await _userManager.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken);
        }

        private List<Claim> GetAccessTokenClaims(string email, string userId, IList<string>? userRole)
        {
            var claims = new List<Claim>();
            userRole?.ToList().ForEach(e => { claims.Add(new Claim(ClaimTypes.Role, e)); });

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf,
                DateTime.UtcNow.AddMonths(10).ToUniversalTime().ToString()));
            claims.Add(new Claim(
                JwtRegisteredClaimNames.Exp,
                DateTime.UtcNow.AddMonths(10).ToUniversalTime().ToString()
            ));

            return claims;
        }



        private string GenerateAccessToken(List<Claim> claims)
        {
            var secretBytes = _applicationConfig.Key;
            var key = new SymmetricSecurityKey(secretBytes);
            const string algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _applicationConfig.Issuer,
                _applicationConfig.Audience,
                claims,
                null,
                null,
                signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}