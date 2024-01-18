using ag.ems.application.Common;
using ag.ems.domain.Entities.Identity;
using cube360.vbs.application.Common.Models.Identity;

namespace ag.ems.application.Interfaces;

public interface IIdentityService
{
    Task<TokenResponse> LoginAsync(string email, string password);

    Task<Result> InviteUserAsync(
        string email,
        string role,
        UserProfile userProfile);

    Task<Result> ChangePasswordAsync(string currentPwd, string password);
    
    Task<Result> UpdateMyProfileAsync(UserProfile request);

    Task<Result> LockUserAsync(string email);

    Task<AppIdentityUser> GetUserAsync(string email);

    Task<List<UserProfile>> GetUsers();
    
    Task<List<Dictionary<string,string>>> GetUsersForDropDown();

    IQueryable<UserProfile> UserWithQuery();

}