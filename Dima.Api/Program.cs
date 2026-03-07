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


app.MapPost(
        "/v1/categories",
        async (CreateCategoryRequest request, ICategoryHandler handler) => await handler.CreateAsync(request))
    .WithName("Categories: Create")
    .WithTags("Categories")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response<Category>>();

app.MapPut(
        "/v1/categories/{id:long}",
        async ([FromRoute] long id, ICategoryHandler handler) =>
        {
            var request = new UpdateCategoryRequest { Id = id };
            return await handler.UpdateAsync(request);
        })
    .WithName("Categories: Update")
    .WithTags("Categories")
    .WithSummary("Atualiza uma categoria")
    .Produces<Response<Category>>();

app.MapDelete(
        "/v1/categories/{id:long}",
        async ([FromRoute] long id, ICategoryHandler handler) =>
        {
            var request = new DeleteCategoryRequest { Id = id };
            return await handler.DeleteAsync(request);
        })
    .WithName("Categories: Delete")
    .WithTags("Categories")
    .WithSummary("Deleta uma categoria")
    .Produces<Response<Category>>();

app.MapGet(
        "/v1/categories/{id:long}",
        async ([FromRoute] long id, ICategoryHandler handler) =>
        {
            var request = new GetCategoryByIdRequest { Id = id };
            return await handler.GetByIdAsync(request);
        })
    .WithName("Categories: Buscar por Id")
    .WithTags("Categories")
    .WithSummary("Buscar uma categoria por id")
    .Produces<Response<Category>>();

app.MapGet(
        "/v1/categories",
        async (ICategoryHandler handler) =>
        {
            var request = new GetAllCategoriesRequest
            {
                UserId = "1234asd"
            };
            return await handler.GetAllAsync(request);
        })
    .WithName("Categories: Buscar todas")
    .WithTags("Categories")
    .WithSummary("Buscar todas categorias")
    .Produces<PagedResponse<List<Category>?>>();

app.Run();
