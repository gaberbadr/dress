using FluentValidation;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {

            RuleFor(v => v.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(v => v.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleFor(v => v.MainImage)
                .NotEmpty().WithMessage("Main image is required.");

            RuleFor(v => v.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }
    }
}
