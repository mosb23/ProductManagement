using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract;
using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Application.Features.Products.Queries.GetProductStatusHistories;

namespace ProductManagement_V2.Controllers
{
    [Authorize]
    [Route("api/product-status-histories")]
    public class ProductStatusHistoriesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProductStatusHistoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "ProductStatusHistoriesView")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<ProductStatusHistoryDto>>>> Get(
            [FromQuery] ProductStatusHistoryQueryContract query)
        {
            var result = await _mediator.Send(new GetProductStatusHistoriesQuery(query));
            return FromResult(result);
        }
    }
}