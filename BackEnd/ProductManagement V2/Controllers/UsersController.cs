using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract.Users;
using ProductManagement_V2.Application.Features.Users.Commands.CreateUser;
using ProductManagement_V2.Application.Features.Users.Queries.GetAllUsers;
using ProductManagement_V2.Application.Features.Users.Queries.GetUserById;

namespace ProductManagement_V2.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "UsersCreate")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Create(CreateUserRequest request)
        {
            var result = await _mediator.Send(new CreateUserCommand(request));
            return FromResult(result, "User created successfully");
        }

        [HttpGet]
        [Authorize(Policy = "UsersView")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<UserResponse>>>> GetAll(
            [FromQuery] UserQueryContract query)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(query));
            return FromResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UsersView")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetById(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return FromResult(result);
        }
    }
}