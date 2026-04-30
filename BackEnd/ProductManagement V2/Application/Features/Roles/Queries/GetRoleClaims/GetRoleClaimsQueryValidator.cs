using FluentValidation;

namespace ProductManagement_V2.Application.Features.Roles.Queries.GetRoleClaims
{
    public class GetRoleClaimsQueryValidator : AbstractValidator<GetRoleClaimsQuery>
    {
        public GetRoleClaimsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Role id is required.");
        }
    }
}