using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PruebaSCI.Application.Interfaces;
using PruebaSCI.Domain.Entities;

namespace PruebaSCI.Infrastructure.Repositories;

public sealed class ProductRepository(IConfiguration configuration) : IProductRepository
{
    private readonly string connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("La cadena de conexión no está configurada.");

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        var command = new CommandDefinition("dbo.Product_GetAll", commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        var products = await connection.QueryAsync<Product>(command);
        return products.ToList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        var command = new CommandDefinition("dbo.Product_GetById", new { Id = id }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<Product>(command);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        var command = new CommandDefinition("dbo.Product_Create", new
        {
            product.Name,
            product.Description,
            product.Price
        }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.QuerySingleAsync<Product>(command);
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        var command = new CommandDefinition("dbo.Product_Update", new
        {
            product.Id,
            product.Name,
            product.Description,
            product.Price
        }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await using var connection = CreateConnection();
        var command = new CommandDefinition("dbo.Product_Delete", new { Id = id }, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken);
        return await connection.ExecuteScalarAsync<int>(command) == 1;
    }

    private SqlConnection CreateConnection() => new(connectionString);
}
