using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Models;
using ProductCatalogue.Services;

namespace ProductCatalogue.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    // GET /api/products
    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    // GET /api/products/1
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _service.GetById(id);
        if (product is null)
            return NotFound(new { message = $"Product with id {id} not found." });

        return Ok(product);
    }

    // GET /api/products/category/Electronics
    [HttpGet("category/{category}")]
    public ActionResult<List<Product>> GetByCategory(string category)
    {
        var products = _service.GetByCategory(category);
        return Ok(products);
    }

    // POST /api/products
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest(new { message = "Product name is required." });

        if (product.Price <= 0)
            return BadRequest(new { message = "Price must be greater than zero." });

        var created = _service.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/products/1
    [HttpPut("{id}")]
    public ActionResult<Product> Update(int id, [FromBody] Product product)
    {
        var updated = _service.Update(id, product);
        if (updated is null)
            return NotFound(new { message = $"Product with id {id} not found." });

        return Ok(updated);
    }

    // DELETE /api/products/1
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var deleted = _service.Delete(id);
        if (!deleted)
            return NotFound(new { message = $"Product with id {id} not found." });

        return NoContent();
    }
}