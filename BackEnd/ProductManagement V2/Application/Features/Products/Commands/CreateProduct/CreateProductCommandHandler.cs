using AutoMapper;
using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Domain.Entities;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.Request);

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(product.Id);
        }
    }
}