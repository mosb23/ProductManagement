using MediatR;
using ProductManagement_V2.Application.Common.Results;
using ProductManagement_V2.Application.Contract.Statistics;

namespace ProductManagement_V2.Application.Features.Statistics.Queries.GetStatistics
{
    public record GetStatisticsQuery : IRequest<Result<StatisticsResponse>>;

}
