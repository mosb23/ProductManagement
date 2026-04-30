using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract;

namespace ProductManagement_V2.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(ProductCreateContract Request)
        : IRequest<Result<int>>;
}
