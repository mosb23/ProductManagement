using MediatR;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract;
using ProductManagement_V2.Application.DTOs.Product;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery(ProductQueryContract Query)
        : IRequest<Result<PaginatedResult<ProductResponseDto>>>;
}