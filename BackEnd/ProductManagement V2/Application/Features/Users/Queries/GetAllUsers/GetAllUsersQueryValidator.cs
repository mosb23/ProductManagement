using FluentValidation;

namespace ProductManagement_V2.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(x => x.Query.PageNumber)
                .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.Query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100.");

            RuleFor(x => x.Query.FullName)
                .MaximumLength(200).WithMessage("FullName cannot exceed 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query.FullName));

            RuleFor(x => x.Query.Email)
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query.Email));

            RuleFor(x => x.Query.Role)
                .MaximumLength(100).WithMessage("Role cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Query.Role));
        }
    }
}