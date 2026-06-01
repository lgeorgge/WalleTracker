using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WalletTracker.Application.Interfaces;
using WalletTracker.Infrastructure.Data;
using WalletTracker.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddOpenApi();

#endregion
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
