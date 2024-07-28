using API.DTOs.ProductDTOs;
using FluentValidation;

namespace API.Validators
{
    public class ProductValidator : AbstractValidator<ProductSaveDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Description).MaximumLength(500);

            RuleFor(p => p.RegularPrice)
                .NotNull()
                .Must(HaveValidDecimalPlaces)
                .WithMessage("The Regular Price field must have at most 2 decimal places.");
            
            RuleFor(p => p.SellingPrice)
                .NotNull()
                .Must(HaveValidDecimalPlaces)
                .WithMessage("The Selling Price field must have at most 2 decimal places.");
        }

        private bool HaveValidDecimalPlaces(decimal? value)
        {
            return decimal.Round(value ?? 0, 2) == value;
        }
    }
}