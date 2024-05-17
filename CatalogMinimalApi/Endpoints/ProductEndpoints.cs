using CatalogMinimalApi.Context;
using CatalogMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        // Produtos
        app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync()).WithTags("Produtos").RequireAuthorization();

        app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Products.FindAsync(id) is Product product
                ? Results.Ok(product)
                : Results.NotFound();
        }).WithTags("Produtos");

        app.MapPost("/products", async (Product product, AppDbContext db) =>
        {
            var category = await db.Categories.FindAsync(product.CategoryId);
            if (category is null) return Results.NotFound("Categoria inválida");

            product.Category = category;

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{product.ProductId}", product);
        }).WithTags("Produtos");

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
        }).WithTags("Produtos");

        app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);

            if (product is null) return Results.NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithTags("Produtos");
    }
}
