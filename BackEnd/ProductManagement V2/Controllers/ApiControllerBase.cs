using Microsoft.AspNetCore.Mvc;
using ProductManagement_V2.Application.Common;
using ProductManagement_V2.Application.Common.Results;

namespace ProductManagement_V2.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> FromResult<T>(
            Result<T> result,
            string successMessage = "Success",
            int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
                return Ok(ApiResponse<T>.SuccessResponse(result.Value!, successMessage, successStatusCode));

            var statusCode = ToHttpStatusCode(result.Status);
            var message = result.Errors.FirstOrDefault() ?? DefaultFailureMessage(result.Status);

            return Ok(ApiResponse<T>.FailResponse(message, statusCode, result.Errors));
        }

        protected ActionResult<ApiResponse<string>> FromResult(
            Result result,
            string successMessage,
            int successStatusCode = StatusCodes.Status200OK)
        {
            if (result.IsSuccess)
                return Ok(ApiResponse<string>.SuccessResponse(null, successMessage, successStatusCode));

            var statusCode = ToHttpStatusCode(result.Status);
            var message = result.Errors.FirstOrDefault() ?? DefaultFailureMessage(result.Status);

            return Ok(ApiResponse<string>.FailResponse(message, statusCode, result.Errors));
        }

        private static int ToHttpStatusCode(ResultStatus status)
        {
            return status switch
            {
                ResultStatus.Success => StatusCodes.Status200OK,
                ResultStatus.Validation => StatusCodes.Status400BadRequest,
                ResultStatus.BadRequest => StatusCodes.Status400BadRequest,
                ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
                ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
                ResultStatus.NotFound => StatusCodes.Status404NotFound,
                ResultStatus.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };
        }

        private static string DefaultFailureMessage(ResultStatus status)
        {
            return status switch
            {
                ResultStatus.Validation => "Validation error",
                ResultStatus.BadRequest => "Bad request",
                ResultStatus.Unauthorized => "Unauthorized",
                ResultStatus.Forbidden => "Forbidden",
                ResultStatus.NotFound => "Not found",
                ResultStatus.Conflict => "Conflict",
                _ => "Request failed"
            };
        }
    }
}
