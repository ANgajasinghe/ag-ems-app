using ag.ems.api;
using ag.ems.application.Common;
using ag.ems.application.Services;
using ag.ems.infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Throw;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<SalaryService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var applicationConfig = builder.Configuration.GetSection(nameof(ApplicationConfig))
    .Get<ApplicationConfig>();
applicationConfig.ThrowIfNull();

builder.Services
    .AddAppIdentity(applicationConfig)
    .AddAppServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(applicationConfig.AllowedHosts)
            .AllowCredentials()
            .WithExposedHeaders("content-disposition"));
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "cube360.vbs.ui",
        Version = "v1"
    });
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Open");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();
