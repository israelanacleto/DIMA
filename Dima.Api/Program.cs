using Dima.Api.Common.Api;
using Dima.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();

// Security
builder.AddSecurity();
// Dependency Injection
builder.AddInfrastructure();
// Cors
builder.AddCrossOrigin();
// Docs
builder.AddDocs();

var app = builder.Build();

app.UseSecurity();

app.ConfigureDevEnvironment();

app.MapEndpoints();


app.Run();
