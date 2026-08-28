using PruebaSCI.Application.DTOs.Products;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Domain.Entities;

namespace PruebaSCI.Application.Services;

public sealed class ProductService(IProductRepository repository) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(Map).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : Map(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await repository.CreateAsync(new Product
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price
        }, cancellationToken);

        return Map(product);
    }

    public Task<bool> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        return repository.UpdateAsync(new Product
        {
            Id = id,
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Price = request.Price
        }, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }

    private static ProductResponse Map(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.CreatedDate);
}
