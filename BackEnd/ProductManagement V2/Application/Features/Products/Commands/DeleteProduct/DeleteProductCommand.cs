using MediatR;
using ProductManagement_V2.Application.Common.Results;

namespace ProductManagement_V2.Application.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<Result>;
}