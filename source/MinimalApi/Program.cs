using Microsoft.AspNetCore.Server.Kestrel.Core;
using MinimalApi.Application;
using MinimalApi.Domain.Models;
using MinimalApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(50000, config => config.Protocols = HttpProtocols.Http1AndHttp2);
    options.ListenLocalhost(50001, config => config.Protocols = HttpProtocols.Http2);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

builder.Services.AddSingleton<ICompaniesRepository, FakerCompaniesRepository>();
builder.Services.AddSingleton<IPersonRepository, FakerPersonRepository>();
builder.Services.AddScoped<ICompanyService, CompanyWebService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/ping", () => "pong").WithName("GetPing");
app.MapGet("/projects", (ICompanyService svc) => svc.GetAllAsync());

app.MapGet("/projects/{id}", async (Guid id, ICompanyService svc) =>
    await svc.GetByIdAsync(id) is Company c
        ? Results.Ok(c)
        : Results.NotFound());

app.MapGrpcService<PersonGrpcService>();

app.Run();
