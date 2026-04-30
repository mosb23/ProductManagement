using FluentValidation;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.Query.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.Query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100.");

            RuleFor(x => x.Query.Name)
                .MinimumLength(3).WithMessage("Name must be at least 3 characters.")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query.Name));

            RuleFor(x => x.Query.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query.Description));

            RuleFor(x => x.Query.Price)
                .GreaterThan(0).WithMessage("Price must be more than 0.")
                .When(x => x.Query.Price.HasValue);

            RuleFor(x => x.Query.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or more.")
                .When(x => x.Query.Quantity.HasValue);
        }
    }
}