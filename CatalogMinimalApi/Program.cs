using CatalogMinimalApi.Context;
using CatalogMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

// /////////////////////// Endpoints ///////////////////////

// Categorias
app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync());

app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categories.FindAsync(id) is Category category
        ? Results.Ok(category)
        : Results.NotFound();
});

app.MapPost("/categories", async (Category category, AppDbContext db) => 
{ 
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.CategoryId}", category);
});

app.MapPut("/categories/{id:int}", async (int id, Category updatedCategory, AppDbContext db) =>
{
    if (updatedCategory.CategoryId != id)
        return Results.BadRequest();

    var category = await db.Categories.FindAsync(id);

    if (category is null) return Results.NotFound();

    category.Name = updatedCategory.Name;
    category.Description = updatedCategory.Description;

    await db.SaveChangesAsync();
    return Results.Ok(category);
});

app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) => 
{
    var category = await db.Categories.FindAsync(id);

    if (category is null) return Results.NotFound();

    db.Categories.Remove(category);
    await db.SaveChangesAsync();
    return Results.Ok(category);
});

// Produtos
app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Products.FindAsync(id) is Product product
        ? Results.Ok(product)
        : Results.NotFound();
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    var category = await db.Categories.FindAsync(product.CategoryId);
    if (category is null) return Results.NotFound("Categoria inválida");

    product.Category = category;

    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{product.ProductId}", product);
});

app.MapPut("/products/{id:int}", async (int id, Product updatedProduct, AppDbContext db) =>
{
    if (updatedProduct.ProductId != id)
        return Results.BadRequest();

    var product = await db.Products.FindAsync(id);

    if (product is null) return Results.NotFound();

    product.Name = updatedProduct.Name;
    product.Description = updatedProduct.Description;
    product.Price = updatedProduct.Price;
    product.ImageUrl = updatedProduct.ImageUrl;
    product.PurchaseDate = updatedProduct.PurchaseDate;
    product.Stock = updatedProduct.Stock;
    product.CategoryId = updatedProduct.CategoryId;

    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// /////////////////////////////////////////////////////////

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();