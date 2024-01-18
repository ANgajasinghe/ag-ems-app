using ag.ems.application.Common;
using ag.ems.domain.Entities.Identity;
using ag.ems.infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ag.ems.api;

public static class ConfigureServices
{
     public static IServiceCollection AddAppIdentity(this IServiceCollection services,ApplicationConfig applicationConfig)
    {
        services.AddIdentity<AppIdentityUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = true;

                config.Password.RequireLowercase = true;
                config.Password.RequiredUniqueChars = 1;

                config.Password.RequiredLength = 8;
                config.Password.RequireNonAlphanumeric = true;

                config.Password.RequireUppercase = true;
                config.User.RequireUniqueEmail = true;

                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton(applicationConfig);

        applicationConfig.Validate();
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(applicationConfig.Key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParams);
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });

        return services;
    }
}