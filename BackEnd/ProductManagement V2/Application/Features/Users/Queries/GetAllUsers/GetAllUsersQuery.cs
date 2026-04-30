using MediatR;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;

namespace ProductManagement_V2.Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(UserQueryContract Query)
        : IRequest<Result<PaginatedResult<UserResponse>>>;
}