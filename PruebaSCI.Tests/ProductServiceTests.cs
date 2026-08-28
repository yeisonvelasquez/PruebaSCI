using PruebaSCI.Application.DTOs.Products;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Application.Services;
using PruebaSCI.Domain.Entities;

namespace PruebaSCI.Tests;

public sealed class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_TrimsTextAndReturnsCreatedProduct()
    {
        var repository = new InMemoryProductRepository
        {
            CreatedProduct = new Product
            {
                Id = 1,
                Name = "Product",
                Description = "Description",
                Price = 7.92m,
                CreatedDate = DateTime.UtcNow
            }
        };
        var service = new ProductService(repository);

        var result = await service.CreateAsync(new CreateProductRequest
        {
            Name = " Product ",
            Description = " Description ",
            Price = 7.92m
        }, CancellationToken.None);

        Assert.Equal(1, result.Id);
        Assert.Equal("Product", repository.LastCreatedProduct!.Name);
        Assert.Equal("Description", repository.LastCreatedProduct.Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
    {
        var service = new ProductService(new InMemoryProductRepository());

        var result = await service.GetByIdAsync(99, CancellationToken.None);

        Assert.Null(result);
    }

    private sealed class InMemoryProductRepository : IProductRepository
    {
        public Product? CreatedProduct { get; init; }
        public Product? LastCreatedProduct { get; private set; }

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<Product>>([]);

        public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult<Product?>(null);

        public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            LastCreatedProduct = product;
            return Task.FromResult(CreatedProduct ?? product);
        }

        public Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken) =>
            Task.FromResult(false);

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken) =>
            Task.FromResult(false);
    }
}
