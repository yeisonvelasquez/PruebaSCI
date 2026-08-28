using System.ComponentModel.DataAnnotations;

namespace PruebaSCI.Application.Validation;

public sealed class PriceRangeAttribute : ValidationAttribute
{
    private const decimal MinimumPrice = 0.01m;
    private const decimal MaximumPrice = 9999999999999999.99m;

    public PriceRangeAttribute()
        : base("El precio debe estar entre 0.01 y 9999999999999999.99.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is decimal price && price is >= MinimumPrice and <= MaximumPrice)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
    }
}
