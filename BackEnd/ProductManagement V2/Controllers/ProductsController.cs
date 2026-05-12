using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract;
using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Application.DTOs.Product;
using ProductManagement_V2.Application.Features.Products.Commands.ChangeProductStatus;
using ProductManagement_V2.Application.Features.Products.Commands.CreateProduct;
using ProductManagement_V2.Application.Features.Products.Commands.DeleteProduct;
using ProductManagement_V2.Application.Features.Products.Queries.GetProductById;
using ProductManagement_V2.Application.Features.Products.Queries.GetProducts;
namespace ProductManagement_V2.Controllers
{
    [Authorize]
    [Route("api/products")]
    public class ProductsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "ProductsCreate")]
        public async Task<ActionResult<ApiResponse<int>>> Create(ProductCreateContract request)
        {
            var result = await _mediator.Send(new CreateProductCommand(request));
            return FromResult(result, "Product Created", StatusCodes.Status201Created);
        }

        [HttpGet]
        [Authorize(Policy = "ProductsView")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<ProductResponseDto>>>> GetProducts(
            [FromQuery] ProductQueryContract query)
        {
            var result = await _mediator.Send(new GetProductsQuery(query));
            return FromResult(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ProductsView")]
        public async Task<ActionResult<ApiResponse<ProductDetailsDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return FromResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ProductsDelete")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return FromResult(result, "Product soft-deleted");
        }

        [HttpPatch("{id}/status")]
        [Authorize(Policy = "ProductsChangeStatus")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateStatus(int id, UpdateProductStatusDto dto)
        {
            var result = await _mediator.Send(new ChangeProductStatusCommand(id, dto.Status));
            return FromResult(result, "Status updated");
        }
    }
}
