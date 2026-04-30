using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Domain.Enums;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProductStatusHistories
{
    public class GetProductStatusHistoriesQueryHandler
        : IRequestHandler<GetProductStatusHistoriesQuery, Result<PaginatedResult<ProductStatusHistoryDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductStatusHistoriesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PaginatedResult<ProductStatusHistoryDto>>> Handle(
            GetProductStatusHistoriesQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Query;

            var historiesQuery = _context.ProductStatusHistories
                .Include(x => x.Product)
                .AsQueryable();

            if (query.ProductId.HasValue)
                historiesQuery = historiesQuery.Where(x => x.ProductId == query.ProductId.Value);

            if (query.OldStatus.HasValue)
                historiesQuery = historiesQuery.Where(x => x.OldStatus == query.OldStatus.Value);

            if (query.NewStatus.HasValue)
                historiesQuery = historiesQuery.Where(x => x.NewStatus == query.NewStatus.Value);

            if (query.FromDate.HasValue)
                historiesQuery = historiesQuery.Where(x => x.CreatedAt >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                historiesQuery = historiesQuery.Where(x => x.CreatedAt <= query.ToDate.Value);

            var totalCount = await historiesQuery.CountAsync(cancellationToken);

            var data = await historiesQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new ProductStatusHistoryDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product != null ? x.Product.Name : null,
                    OldStatus = (ProductStatus)x.OldStatus,
                    NewStatus = (ProductStatus)x.NewStatus,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<ProductStatusHistoryDto>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };

            return Result<PaginatedResult<ProductStatusHistoryDto>>.Success(result);
        }
    }
}