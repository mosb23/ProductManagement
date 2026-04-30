using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Users;

namespace ProductManagement_V2.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string Id) : IRequest<Result<UserResponse>>;
}