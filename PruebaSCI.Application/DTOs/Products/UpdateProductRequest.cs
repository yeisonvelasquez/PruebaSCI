using System.ComponentModel.DataAnnotations;
using PruebaSCI.Application.Validation;

namespace PruebaSCI.Application.DTOs.Products;

public sealed class UpdateProductRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MaxLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
    public string Description { get; init; } = string.Empty;

    [PriceRange]
    public decimal Price { get; init; }
}
