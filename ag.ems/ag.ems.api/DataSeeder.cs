using ag.ems.domain.Const;
using ag.ems.domain.Entities.Identity;
using ag.ems.infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ag.ems.api;

public class DataSeeder
{
    private readonly AppDbContext _appDbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppIdentityUser> _userManager;

    public DataSeeder(AppDbContext appDbContext, RoleManager<IdentityRole> roleManager, UserManager<AppIdentityUser> userManager)
    {
        _appDbContext = appDbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    public  async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_appDbContext.Database.IsSqlServer()) await _appDbContext.Database.MigrateAsync(cancellationToken: stoppingToken);
        await TrySeedAsync();
    }

    private async Task TrySeedAsync()
    {

        var roles = new List<IdentityRole>
        {

            new IdentityRole(RoleConstant.Admin),
            new IdentityRole(RoleConstant.Employee),
          
        };

        
        var users = new List<AppIdentityUser>
        {
            new AppIdentityUser("admin@admin.com", "admin")
        };

        
        
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                await _roleManager.CreateAsync(role);
            }
        }
        
        foreach (var user in users)
        {
            var userInDb = await _userManager.FindByEmailAsync(user.Email);
            if (userInDb is null)
            {
                var ret = await _userManager.CreateAsync(user, "#Admin@123456");

                switch (user.Email)
                {
                    case "admin@admin.com":
                        await _userManager.AddToRolesAsync(user, new[] { RoleConstant.Admin });
                        break;
                }
            }
        }
    }
}