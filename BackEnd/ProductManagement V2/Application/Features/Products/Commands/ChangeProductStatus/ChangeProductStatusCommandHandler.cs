using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Application.Common.Auth;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Infrastructuree;

namespace ProductManagement_V2.Application.Features.Products.Commands.ChangeProductStatus
{
    public class ChangeProductStatusCommandHandler
        : IRequestHandler<ChangeProductStatusCommand, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ChangeProductStatusCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(
            ChangeProductStatusCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .Include(p => p.StatusHistories)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product is null)
                return Result.NotFound("Product not found");

            try
            {
                product.UpdateStatus(
                    request.Status,
                    _currentUserService.UserId,
                    _currentUserService.DisplayName);
            }
            catch (Exception ex)
            {
                return Result.BadRequest(ex.Message);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
