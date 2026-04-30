using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Contract.Roles;
using ProductManagement_V2.Application.Features.Roles.Queries.GetAllRoles;
using ProductManagement_V2.Application.Features.Roles.Queries.GetRoleClaims;

namespace ProductManagement_V2.Controllers
{
    [Authorize]
    [Route("api/roles")]
    public class RolesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "RolesView")]
        public async Task<ActionResult<ApiResponse<PaginatedResult<RoleResponse>>>> GetAll(
            [FromQuery] RoleQueryContract query)
        {
            var result = await _mediator.Send(new GetAllRolesQuery(query));
            return FromResult(result);
        }

        [HttpGet("{id}/claims")]
        [Authorize(Policy = "RolesView")]
        public async Task<ActionResult<ApiResponse<List<RoleClaimResponse>>>> GetClaims(string id)
        {
            var result = await _mediator.Send(new GetRoleClaimsQuery(id));
            return FromResult(result);
        }
    }
}