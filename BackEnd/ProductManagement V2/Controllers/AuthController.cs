using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract.Auth;
using ProductManagement_V2.Application.Features.Auth.Commands.Login;
using ProductManagement_V2.Application.Features.Auth.Commands.RefreshToken;

namespace ProductManagement_V2.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(request));
            return FromResult(result, "Login successful");
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Refresh(RefreshTokenRequest request)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(request));
            return FromResult(result, "Token refreshed successfully");
        }
    }
}