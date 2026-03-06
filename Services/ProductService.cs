using ProductCatalogue.Models;

namespace ProductCatalogue.Services;

public class ProductService
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop Pro", Category = "Electronics", Description = "High performance laptop", Price = 999.99m, Stock = 50 },
        new Product { Id = 2, Name = "Wireless Mouse", Category = "Electronics", Description = "Ergonomic wireless mouse", Price = 29.99m, Stock = 200 },
        new Product { Id = 3, Name = "Office Chair", Category = "Furniture", Description = "Comfortable office chair", Price = 249.99m, Stock = 30 },
        new Product { Id = 4, Name = "Standing Desk", Category = "Furniture", Description = "Adjustable standing desk", Price = 499.99m, Stock = 20 },
        new Product { Id = 5, Name = "Notebook", Category = "Stationery", Description = "A4 ruled notebook", Price = 4.99m, Stock = 500 },
    };

    private int _nextId = 6;

    public List<Product> GetAll() => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public List<Product> GetByCategory(string category) =>
        _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

    public Product Add(Product product)
    {
        product.Id = _nextId++;
        product.CreatedAt = DateTime.UtcNow;
        _products.Add(product);
        return product;
    }

    public Product? Update(int id, Product updated)
    {
        var existing = GetById(id);
        if (existing is null) return null;

        existing.Name = updated.Name;
        existing.Category = updated.Category;
        existing.Description = updated.Description;
        existing.Price = updated.Price;
        existing.Stock = updated.Stock;

        return existing;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product is null) return false;
        _products.Remove(product);
        return true;
    }
}