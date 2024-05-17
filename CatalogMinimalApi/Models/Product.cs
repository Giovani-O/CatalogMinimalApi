﻿using System.Text.Json.Serialization;

namespace CatalogMinimalApi.Models;

public class Product
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public  string? ImageUrl { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Stock { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
}
