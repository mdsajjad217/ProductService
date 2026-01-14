using FluentValidation;
using ProductService.Domain.Dto;

namespace ProductService.Domain.Validator
{
    public class CreateProductRequestValidator
        : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}