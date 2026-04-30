using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
                return Result.NotFound("Product not found");

            if (product.IsDeleted)
                return Result.Conflict("Product already deleted");

            _context.Products.Remove(product);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}