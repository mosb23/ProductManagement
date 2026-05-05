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

                    History = _context.ProductStatusHistories
                        .Where(h => h.ProductId == p.Id)
                        .OrderByDescending(h => h.CreatedAt)
                        .GroupJoin(
                            _context.UsersWithRoles,
                            history => history.CreatedByUserId,
                            user => user.Id,
                            (history, users) => new { history, userById = users.FirstOrDefault() })
                        .GroupJoin(
                            _context.UsersWithRoles,
                            x => x.history.CreatedBy,
                            user => user.FullName,
                            (x, users) => new
                            {
                                x.history,
                                user = x.userById ?? users.FirstOrDefault()
                            })
                        .Select(h => new ProductStatusHistoryDto
                        {
                            Id = h.history.Id,
                            ProductId = h.history.ProductId,
                            ProductName = p.Name,

                            OldStatus = h.history.OldStatus,
                            NewStatus = h.history.NewStatus,

                            CreatedAt = h.history.CreatedAt,
                            CreatedBy = h.history.CreatedBy,
                            CreatedByRole = h.user != null ? h.user.RoleName : null
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product is null)
                return Result<ProductDetailsDto>.NotFound("Product not found");

            return Result<ProductDetailsDto>.Success(product);
        }
    }
}
