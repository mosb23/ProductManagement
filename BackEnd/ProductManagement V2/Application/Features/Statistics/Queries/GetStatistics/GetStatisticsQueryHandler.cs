using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Statistics;
using ProductManagement_V2.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace ProductManagement_V2.Application.Features.Statistics.Queries.GetStatistics
{
    public class GetStatisticsQueryHandler
           : IRequestHandler<GetStatisticsQuery, Result<StatisticsResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetStatisticsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<StatisticsResponse>> Handle(
            GetStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var productStatusCounts = await _context.Products
                .GroupBy(p => p.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var userStatusCounts = await _context.Users
                .GroupBy(u => u.IsActive)
                .Select(g => new
                {
                    IsActive = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var statusChangesTotal = await _context.ProductStatusHistories
                .CountAsync(cancellationToken);

            var response = new StatisticsResponse
            {
                Products = new ProductStatisticsResponse
                {
                    Total = productStatusCounts.Sum(x => x.Count),

                    Available = productStatusCounts
                        .FirstOrDefault(x => x.Status == ProductStatus.Available)?.Count ?? 0,

                    OutOfStock = productStatusCounts
                        .FirstOrDefault(x => x.Status == ProductStatus.OutOfStock)?.Count ?? 0,

                    Discontinued = productStatusCounts
                        .FirstOrDefault(x => x.Status == ProductStatus.Discontinued)?.Count ?? 0
                },

                StatusChanges = new StatusChangeStatisticsResponse
                {
                    Total = statusChangesTotal
                },

                Users = new UserStatisticsResponse
                {
                    Total = userStatusCounts.Sum(x => x.Count),

                    Active = userStatusCounts
                        .FirstOrDefault(x => x.IsActive)?.Count ?? 0,

                    Inactive = userStatusCounts
                        .FirstOrDefault(x => !x.IsActive)?.Count ?? 0
                }
            };

            return Result<StatisticsResponse>.Success(response);
        }
    }
}
