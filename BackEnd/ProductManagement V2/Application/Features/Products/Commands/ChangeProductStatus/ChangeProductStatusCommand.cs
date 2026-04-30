using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Application.Features.Products.Commands.ChangeProductStatus
{
    public record ChangeProductStatusCommand(int Id, ProductStatus Status)
        : IRequest<Result>;
}