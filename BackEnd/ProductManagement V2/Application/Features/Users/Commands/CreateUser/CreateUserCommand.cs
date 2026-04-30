using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;

namespace ProductManagement_V2.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(CreateUserRequest Request)
        : IRequest<Result<UserResponse>>;
}