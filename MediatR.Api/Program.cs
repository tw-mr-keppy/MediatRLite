using System.Reflection;
using MediatR.Application;
using MediatRLite.Core.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var mediatrAssemblies = new[]
{
    Assembly.GetAssembly(typeof(HelloWorldCommandHandler)), // UseCases
};

builder.Services.AddMediatRLite(mediatrAssemblies!);

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MediatR.Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediatR.Api v1"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();