using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract.Statistics;
using ProductManagement_V2.Application.Features.Statistics.Queries.GetStatistics;

namespace ProductManagement_V2.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "StatisticsView")]
        public async Task<ActionResult<ApiResponse<StatisticsResponse>>> Get()
        {
            var result = await _mediator.Send(new GetStatisticsQuery());
            return FromResult(result);
        }
    }
}