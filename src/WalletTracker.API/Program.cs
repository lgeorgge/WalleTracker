using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using WalletTracker.API.Exceptions;
using WalletTracker.Application.Common.Auth;
using WalletTracker.Application.Features.Auth;
using WalletTracker.Application.Features.Users;
using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Entities;
using WalletTracker.Infrastructure.Auth;
using WalletTracker.Infrastructure.Data;
using WalletTracker.Infrastructure.Repositories;

// Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] {Message:lj}{NewLine}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Host.UseSerilog();

#region Services


// WalletDbContext
builder.Services.AddDbContext<WalletDBContext>(Options =>
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Generic Repo
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();

// Jwt settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOpenApi();

// Password Managers
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); // microsoft's
builder.Services.AddScoped<IPasswordService, PasswordService>(); // our wrapper

// Add Authentication and Authorization
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(O =>
    {
        var settings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
        O.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = settings!.Audience,
            ValidIssuer = settings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
        };

        O.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthenticated",
                        Detail = context.Error,
                        Instance = context.Request.Path,
                    }
                );
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Detail = "Invalid Role",
                        Instance = context.Request.Path,
                    }
                );
            },
        };
    });
builder.Services.AddAuthorization();

// Add current user
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// Add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSerilogRequestLogging(L =>
        L.MessageTemplate = "{RequestMethod} {RequestPath} => {StatusCode} in {Elapsed:0.0000} ms"
    );
}

app.UseHttpsRedirection();

// Use exception handler( NOTE : it should be early in the pipeline)
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
