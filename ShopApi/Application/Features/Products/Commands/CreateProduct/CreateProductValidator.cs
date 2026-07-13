using FluentValidation;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductValidator
       : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}