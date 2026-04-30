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
                        .Select(h => new ProductStatusHistoryDto
                        {
                            Id = h.Id,
                            ProductId = h.ProductId,
                            ProductName = p.Name,

                            OldStatus = h.OldStatus,
                            NewStatus = h.NewStatus,

                            CreatedAt = h.CreatedAt,
                            CreatedBy = h.CreatedBy
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