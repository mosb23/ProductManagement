using MediatR;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract;
using ProductManagement_V2.Application.DTOs;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProductStatusHistories
{
    public record GetProductStatusHistoriesQuery(ProductStatusHistoryQueryContract Query)
        : IRequest<Result<PaginatedResult<ProductStatusHistoryDto>>>;
}