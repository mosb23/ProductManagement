using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Auth;

namespace ProductManagement_V2.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(LoginRequest Request)
        : IRequest<Result<LoginResponse>>;
}
