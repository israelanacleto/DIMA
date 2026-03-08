using Dima.Api.Endpoints;
using Dima.Api.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

// Dependency Injection
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.AddScalarConfig();
}

app.MapGet("/", () => new { Message = "Ok" });
app.MapEndpoints();

app.Run();
