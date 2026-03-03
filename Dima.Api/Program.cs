using Dima.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.AddScalarConfig();
}

app.MapGet("/", () => "Hello World!");

app.Run();
