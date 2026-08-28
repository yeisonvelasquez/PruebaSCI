namespace PruebaSCI.Application.DTOs.Products;

public sealed record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    DateTime CreatedDate);
