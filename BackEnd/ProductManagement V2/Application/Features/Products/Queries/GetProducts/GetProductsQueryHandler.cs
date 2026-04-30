using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.DTOs.Product;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler
        : IRequestHandler<GetProductsQuery, Result<PaginatedResult<ProductResponseDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedResult<ProductResponseDto>>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var query = request.Query;

            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productsQuery = productsQuery
                    .Where(p => p.Name.ToLower().Contains(query.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                productsQuery = productsQuery
                    .Where(p => p.Description != null &&
                                p.Description.ToLower().Contains(query.Description.ToLower()));
            }

            if (query.Price.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.Price == query.Price.Value);
            }

            if (query.Quantity.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.Quantity == query.Quantity.Value);
            }

            var totalCount = await productsQuery.CountAsync(cancellationToken);

            var products = await productsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var mapped = _mapper.Map<List<ProductResponseDto>>(products);

            var result = new PaginatedResult<ProductResponseDto>
            {
                Data = mapped,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };

            return Result<PaginatedResult<ProductResponseDto>>.Success(result);
        }
    }
}