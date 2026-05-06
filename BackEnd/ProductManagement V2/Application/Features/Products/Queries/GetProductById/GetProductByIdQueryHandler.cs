using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Application.DTOs.Product;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, Result<ProductDetailsDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetProductByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductDetailsDto>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDetailsDto
                {
                    Id = p.Id,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy,

                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Status = p.Status,

                    History = new List<ProductStatusHistoryDto>()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product is null)
                return Result<ProductDetailsDto>.NotFound("Product not found");

            var histories = await _context.ProductStatusHistories
                .Where(h => h.ProductId == product.Id)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync(cancellationToken);

            var userIds = histories
                .Where(h => h.CreatedByUserId != null)
                .Select(h => h.CreatedByUserId)
                .Distinct()
                .ToList();

            var createdByNames = histories
                .Where(h => !string.IsNullOrWhiteSpace(h.CreatedBy))
                .Select(h => h.CreatedBy)
                .Distinct()
                .ToList();

            var users = await _context.UsersWithRoles
                .Where(u =>
                    userIds.Contains(u.Id) ||
                    createdByNames.Contains(u.FullName))
                .ToListAsync(cancellationToken);

            product.History = histories.Select(h =>
            {
                var user = users.FirstOrDefault(u => u.Id == h.CreatedByUserId)
                           ?? users.FirstOrDefault(u => u.FullName == h.CreatedBy);

                return new ProductStatusHistoryDto
                {
                    Id = h.Id,
                    ProductId = h.ProductId,
                    ProductName = product.Name,

                    OldStatus = h.OldStatus,
                    NewStatus = h.NewStatus,

                    CreatedAt = h.CreatedAt,
                    CreatedBy = h.CreatedBy,
                    CreatedByRole = user?.RoleName
                };
            }).ToList();

            return Result<ProductDetailsDto>.Success(product);
        }
    }
}
