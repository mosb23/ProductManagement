using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;

namespace ProductManagement_V2.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> FromResult<T>(Result<T> result, string successMessage = "Success")
        {
            if (result.IsSuccess)
                return Ok(ApiResponse<T>.SuccessResponse(result.Value!, successMessage));

            return result.Status switch
            {
                ResultStatus.NotFound => NotFound(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Not found")),
                ResultStatus.Conflict => Conflict(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Conflict")),
                ResultStatus.Validation => BadRequest(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Validation error")),
                ResultStatus.BadRequest => BadRequest(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Bad request")),
                ResultStatus.Unauthorized => Unauthorized(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Unauthorized")),
                ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Forbidden")),
                _ => BadRequest(ApiResponse<T>.FailResponse(result.Errors.FirstOrDefault() ?? "Request failed"))
            };
        }

        protected ActionResult<ApiResponse<string>> FromResult(Result result, string successMessage)
        {
            if (result.IsSuccess)
                return Ok(ApiResponse<string>.SuccessResponse(null, successMessage));

            return result.Status switch
            {
                ResultStatus.NotFound => NotFound(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Not found")),
                ResultStatus.Conflict => Conflict(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Conflict")),
                ResultStatus.Validation => BadRequest(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Validation error")),
                ResultStatus.BadRequest => BadRequest(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Bad request")),
                ResultStatus.Unauthorized => Unauthorized(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Unauthorized")),
                ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Forbidden")),
                _ => BadRequest(ApiResponse<string>.FailResponse(result.Errors.FirstOrDefault() ?? "Request failed"))
            };
        }
    }
}