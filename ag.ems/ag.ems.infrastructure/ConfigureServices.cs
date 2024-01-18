using ag.ems.application.Interfaces;
using ag.ems.infrastructure.Persistence;
using ag.ems.infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ag.ems.infrastructure;

public static class ConfigureServices
{
     public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
     {

         services.AddSqlServer<AppDbContext>(
             configuration.GetConnectionString("DefaultConnection"),
             optionsAction: opt =>
         {
             opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
             opt.LogTo(Console.WriteLine);
         }, sqlServerOptionsAction: builder =>
         {
             builder.MigrationsAssembly("ag.ems.api");
             builder.CommandTimeout(120);
         });
         
      
         
         services.AddHttpContextAccessor();
         services.AddScoped<IJwtTokenService, JwtTokenService>();
         services.AddScoped<IIdentityService, IdentityService>();
         services.AddScoped<ICurrentUserService, CurrentUserService>();
         services.AddScoped<IEmailService, EmailService>();
         services.AddScoped<IAppDbContext, AppDbContext>();
            
        
        return services;    
    }
}