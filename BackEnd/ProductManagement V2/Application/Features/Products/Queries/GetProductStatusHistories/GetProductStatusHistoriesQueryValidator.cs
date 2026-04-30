using FluentValidation;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProductStatusHistories
{
    public class GetProductStatusHistoriesQueryValidator : AbstractValidator<GetProductStatusHistoriesQuery>
    {
        public GetProductStatusHistoriesQueryValidator()
        {
            RuleFor(x => x.Query.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.Query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100.");

            RuleFor(x => x.Query.ProductId)
                .GreaterThan(0).When(x => x.Query.ProductId.HasValue)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Query)
                .Must(q => !q.FromDate.HasValue || !q.ToDate.HasValue || q.FromDate <= q.ToDate)
                .WithMessage("FromDate must be less than or equal to ToDate.");
        }
    }
}