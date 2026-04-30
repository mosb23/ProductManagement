using FluentValidation;
using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Application.Features.Products.Commands.ChangeProductStatus
{
    public class ChangeProductStatusCommandValidator : AbstractValidator<ChangeProductStatusCommand>
    {
        public ChangeProductStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Status)
                .Must(status => Enum.IsDefined(typeof(ProductStatus), status))
                .WithMessage("Invalid product status.");
        }
    }
}