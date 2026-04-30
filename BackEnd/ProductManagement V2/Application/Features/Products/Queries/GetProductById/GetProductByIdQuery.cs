using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.DTOs.Product;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int Id)
        : IRequest<Result<ProductDetailsDto>>;
}