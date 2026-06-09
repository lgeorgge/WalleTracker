using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
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

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
