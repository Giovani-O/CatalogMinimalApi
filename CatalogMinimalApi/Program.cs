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

    return Results.Created($"/categories/{category.Id}", category);
});

app.MapPut("/categories/{id:int}", async (int id, Category updatedCategory, AppDbContext db) =>
{
    if (updatedCategory.Id != id)
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

// /////////////////////////////////////////////////////////

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();