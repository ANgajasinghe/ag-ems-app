using ag.ems.application.Common;
using ag.ems.application.Common.Helpers;
using ag.ems.application.Interfaces;
using ag.ems.domain.Const;
using ag.ems.domain.Entities.Identity;
using ag.ems.infrastructure.Common;
using ag.ems.infrastructure.Persistence;
using cube360.vbs.application.Common.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace ag.ems.infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<AppIdentityUser> _userManager;

#pragma warning disable
        public IdentityService(
            ICurrentUserService currentUserService,
            UserManager<AppIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext,
            IJwtTokenService tokenService,
            IEmailService emailService
        )
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _tokenService = tokenService;
            _emailService = emailService;
        }
#pragma warning restore

        public async Task<TokenResponse> LoginAsync(string email, string password)
        {
            var user = await GetUserAsync(email);
            user.ThrowIfNull("Invalid user email", null);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            var isLock = await _userManager.IsLockedOutAsync(user);
            isLock.Throw("User is locked out", null).IfTrue();
            isPasswordValid.Throw("Invalid password").IfFalse();

            var token = await GetTokenAsync(user);
            return token;
        }
        
        
        public async Task<Result> LockUserAsync(string email)
        {
            var user = await GetUserAsync(email);
            await _userManager.SetLockoutEnabledAsync(user, true);
            var ret = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            return ret.ToApplicationResult();
        }
        public async Task<string> GetUserNameAsync(string? userId)
        {
            var user = await _userManager.Users.Where(x => x.Id == userId)
                .Select(x => x.FullName)
                .FirstOrDefaultAsync();

            return user ?? "No user exist for the given user id";
        }

        public async Task<string> GetUserIdAsync(string email)
        {
            var userId = await _userManager.Users.Where(x => x.Email == email)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            userId.ThrowIfNull("No user exist for the given user email");

            return userId;
        }


        public async Task<Result> InviteUserAsync(
            string email, 
            string role,
            UserProfile userProfile)
        {
            try
            {
                var isEmailAlreadyTaken = await _userManager.Users.AnyAsync(x => x.Email == email);
                if (isEmailAlreadyTaken)
                    return Result.Failure("Email already taken");

                if (!await _roleManager.RoleExistsAsync(role))
                    return Result.Failure("Invalid role");

                var user = new AppIdentityUser(email, "")
                {
                    Address = userProfile.Address,
                    FullName = userProfile.FullName,
                    Telephone = userProfile.Telephone,
                    Salary = userProfile.Salary,
                    JoinDate = userProfile.JoinDate
                };


                var password = PasswordHelper.GeneratePassword();
                password.ThrowIfNull();

                var userCreationData = await _userManager.CreateAsync(user, password);
                if (!userCreationData.Succeeded)
                    return userCreationData.ToApplicationResult();
                await _userManager.AddToRoleAsync(user, role);
                _emailService.SendUserInvitationEmail(password, email, role);
                return userCreationData.ToApplicationResult();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);

            }

        }


        public async Task<Result> UpdateMyProfileAsync(UserProfile request)
        {
            var user = await GetUserAsync(_currentUserService.UserEmail);
            user.ThrowIfNull("No user exists", null);
            user.Email
                .ThrowIfNull()
                .Throw("Invalid user update").IfNotEquals(request.Email);
            
            user.Address = request.Address;
            user.Telephone = request.Telephone;
            user.FullName = request.FullName;

            var ret = await _userManager.UpdateAsync(user);
            
            return ret.ToApplicationResult();
            
        }

        public async Task<UserProfile?> GetMyProfileAsync()
        {
            return await _userManager.Users
                .AsSingleQuery()
                .Where(x => x.Id == _currentUserService.UserId)
                .Select(x => new UserProfile
                {
                    Email = x.Email,
                    FullName = x.FullName,
                    Address = x.Address,
                    Telephone = x.Telephone
                })
                .FirstOrDefaultAsync();
        }


        public async Task<Result> ChangePasswordAsync(string currentPwd, string password)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            currentPwd.Throw("Current password and new password can't be same")
                .IfEquals(password);

            var currentPwdCheckResult = await _userManager.CheckPasswordAsync(user, currentPwd);
            if (!currentPwdCheckResult) return Result.Failure("Current password is invalid!");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, password);

            return res.ToApplicationResult();
        }

        public async Task<TokenResponse> GenerateTokenFromRefreshTokenAsync(string refreshToken)
        {
            var user = await GetUserAsync(_currentUserService.UserEmail);
            var isRefreshTokenResult = await _tokenService.ValidateRefreshTokenAsync(user, refreshToken);

            isRefreshTokenResult.Throw("Invalid refresh token", null).IfFalse();

            var token = await GetTokenAsync(user);
            return new TokenResponse
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken
            };
        }


        public async Task<AppIdentityUser> GetUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.ThrowIfNull("Invalid user email", null);
            return user;
        }

        public Task<List<UserProfile>> GetUsers()
        {
           return UserWithQuery().ToListAsync();
        }

        public async Task<List<Dictionary<string, string>>> GetUsersForDropDown()
        {
           var data = await UserWithQuery()
                .Select(x => new Dictionary<string, string>
                {
                    {"label", x.FullName},
                    {"value", x.Email}
                })
                .ToListAsync();

           return data;
        }

        public IQueryable<UserProfile> UserWithQuery() 
        {
            return _appDbContext.Users
                .Join(
                    _appDbContext.UserRoles, 
                    x=>x.Id,
                    y=>y.UserId,
                    (x,y) => new {
                        User = x, 
                        Role = y
                    }
                )
                .Join(
                    _appDbContext.Roles,
                    x=>x.Role.RoleId,
                    y=>y.Id,
                    (x,y) => new {
                        x.User,
                        Role = y.Name
                    }
                )
                .Where(x=>x.Role != RoleConstant.Admin)
                .Select(x => new UserProfile
                {
                    Id = x.User.Id,
                    Email = x.User.Email,
                    FullName = x.Role,
                    Address = x.User.Address,
                    Telephone = x.User.Telephone,
                    Salary = x.User.Salary,
                    JoinDate =  x.User.JoinDate,
                    Lock =  x.User.Lock
                });

        }

       

        private async Task<TokenResponse> GetTokenAsync(AppIdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);


            return new TokenResponse
            {
                AccessToken = _tokenService.GetAccessToken(user.Email, user.Id, roles),
                RefreshToken = await _tokenService.GenerateRefreshTokenAsync(user)
            };
        }
    }
}
