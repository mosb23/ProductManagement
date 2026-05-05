using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Domain.Enums;

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

            var histories = await historiesQuery
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new
                {
                    x.Id,
                    x.ProductId,
                    ProductName = x.Product != null ? x.Product.Name : null,
                    x.OldStatus,
                    x.NewStatus,
                    x.CreatedAt,
                    x.CreatedBy,
                    x.CreatedByUserId
                })
                .ToListAsync(cancellationToken);

            var userIds = histories
                .Where(x => !string.IsNullOrWhiteSpace(x.CreatedByUserId))
                .Select(x => x.CreatedByUserId!)
                .Distinct()
                .ToList();

            var users = await _context.UsersWithRoles
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    u.RoleName
                })
                .ToListAsync(cancellationToken);

            var usersLookup = users
                .GroupBy(u => u.Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().RoleName
                );

            var data = histories.Select(x => new ProductStatusHistoryDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                OldStatus = (ProductStatus)x.OldStatus,
                NewStatus = (ProductStatus)x.NewStatus,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                CreatedByRole = x.CreatedByUserId != null &&
                                usersLookup.TryGetValue(x.CreatedByUserId, out var roleName)
                    ? roleName
                    : null
            }).ToList();

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