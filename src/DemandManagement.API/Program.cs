using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using DemandManagement.Infrastructure.Repositories;
using DemandManagement.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registrar controllers
builder.Services.AddControllers();

// Registrar MediatR (apunta a la assembly de Application)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DemandManagement.Application.Requests.CreateDemandCommand).Assembly));

// Registrar repositorio (InMemory demo)
builder.Services.AddSingleton<IDemandRepository, InMemoryDemandRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
