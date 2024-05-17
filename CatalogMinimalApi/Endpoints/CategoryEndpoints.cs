using CatalogMinimalApi.Context;
using CatalogMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalApi.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        // Categorias
        app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync()).WithTags("Categorias").RequireAuthorization();

        app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Categories.FindAsync(id) is Category category
                ? Results.Ok(category)
                : Results.NotFound();
        }).WithTags("Categorias");

        app.MapPost("/categories", async (Category category, AppDbContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{category.CategoryId}", category);
        }).WithTags("Categorias");

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
        }).WithTags("Categorias");

        app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) =>
        {
            var category = await db.Categories.FindAsync(id);

            if (category is null) return Results.NotFound();

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Results.Ok(category);
        }).WithTags("Categorias");
    }
}
