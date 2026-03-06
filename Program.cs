using ProductCatalogue.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddSingleton<ProductService>();

// Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Product Catalogue API",
        Version = "v1",
        Description = "A simple Product Catalogue REST API — built to practice CI/CD with Azure DevOps and AKS"
    });
});

var app = builder.Build();

// Always enable Swagger (useful in all environments for this demo)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalogue API v1");
    c.RoutePrefix = string.Empty; // Swagger at root URL
});

app.UseAuthorization();
app.MapControllers();

app.Run();