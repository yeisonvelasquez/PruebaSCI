using Microsoft.AspNetCore.Mvc;
using PruebaSCI.Application.DTOs.Products;
using PruebaSCI.Application.Interfaces;

namespace PruebaSCI.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    /// <summary>Obtiene todos los productos.</summary>
    /// <response code="200">La lista de productos fue obtenida correctamente.</response>
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await productService.GetAllAsync(cancellationToken));
    }

    /// <summary>Obtiene un producto por su identificador.</summary>
    /// <response code="200">El producto fue encontrado.</response>
    /// <response code="404">El producto no existe.</response>
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        return product is null
            ? NotFound(new { message = "El producto solicitado no existe." })
            : Ok(product);
    }

    /// <summary>Crea un producto nuevo.</summary>
    /// <response code="201">El producto fue creado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await productService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>Actualiza un producto existente.</summary>
    /// <response code="204">El producto fue actualizado correctamente.</response>
    /// <response code="400">Los datos enviados no son válidos.</response>
    /// <response code="404">El producto no existe.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await productService.UpdateAsync(id, request, cancellationToken);
        return updated
            ? NoContent()
            : NotFound(new { message = "El producto solicitado no existe." });
    }

    /// <summary>Elimina un producto.</summary>
    /// <response code="204">El producto fue eliminado correctamente.</response>
    /// <response code="404">El producto no existe.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await productService.DeleteAsync(id, cancellationToken);
        return deleted
            ? NoContent()
            : NotFound(new { message = "El producto solicitado no existe." });
    }
}
