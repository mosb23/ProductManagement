namespace ProductManagement_V2.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data, string message = "Success", int statusCode = 200)
            => new() { Success = true, StatusCode = statusCode, Data = data, Message = message };

        public static ApiResponse<T> FailResponse(string message, int statusCode = 400, List<string>? errors = null)
            => new()
            {
                Success = false,
                StatusCode = statusCode,
                Data = default,
                Message = message,
                Errors = errors ?? new List<string> { message }
            };
    }
}
