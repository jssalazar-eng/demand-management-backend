using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using MediatR;
using DemandManagement.Persistence.Data;
using DemandManagement.Persistence.Repositories;
using DemandManagement.Domain.Repositories;
using DemandManagement.Application.Behaviors;
using DemandManagement.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ? Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Frontend origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<DemandManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registrar MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(DemandManagement.Application.Requests.CreateDemandCommand).Assembly);
    // Registrar pipeline de validación
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(DemandManagement.Application.Requests.CreateDemandCommand).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Registrar middleware de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ? Usar CORS (debe ir ANTES de UseAuthorization)
app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
