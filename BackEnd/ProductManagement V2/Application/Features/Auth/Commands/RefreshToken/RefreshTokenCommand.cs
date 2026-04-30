using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Auth;

namespace ProductManagement_V2.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(RefreshTokenRequest Request)
        : IRequest<Result<LoginResponse>>;
}
