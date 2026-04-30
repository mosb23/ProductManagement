using FluentValidation;

namespace ProductManagement_V2.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Request.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
