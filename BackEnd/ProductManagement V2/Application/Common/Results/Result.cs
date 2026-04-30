namespace ProductManagement_V2.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ResultStatus Status { get; }
        public List<string> Errors { get; }

        protected Result(bool isSuccess, ResultStatus status, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            Status = status;
            Errors = errors ?? new List<string>();
        }

        public static Result Success()
            => new(true, ResultStatus.Success);

        public static Result BadRequest(string error)
            => new(false, ResultStatus.BadRequest, new List<string> { error });

        public static Result BadRequest(List<string> errors)
            => new(false, ResultStatus.BadRequest, errors);

        public static Result NotFound(string error)
            => new(false, ResultStatus.NotFound, new List<string> { error });

        public static Result Conflict(string error)
            => new(false, ResultStatus.Conflict, new List<string> { error });

        public static Result Validation(List<string> errors)
            => new(false, ResultStatus.Validation, errors);

        public static Result Unauthorized(string error)
            => new(false, ResultStatus.Unauthorized, new List<string> { error });

        public static Result Forbidden(string error)
            => new(false, ResultStatus.Forbidden, new List<string> { error });
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value)
            : base(true, ResultStatus.Success)
        {
            Value = value;
        }

        private Result(ResultStatus status, List<string> errors)
            : base(false, status, errors)
        {
            Value = default;
        }

        public static Result<T> Success(T value)
            => new(value);

        public static Result<T> BadRequest(string error)
            => new(ResultStatus.BadRequest, new List<string> { error });

        public static Result<T> BadRequest(List<string> errors)
            => new(ResultStatus.BadRequest, errors);

        public static Result<T> NotFound(string error)
            => new(ResultStatus.NotFound, new List<string> { error });

        public static Result<T> Conflict(string error)
            => new(ResultStatus.Conflict, new List<string> { error });

        public static Result<T> Validation(List<string> errors)
            => new(ResultStatus.Validation, errors);

        public static Result<T> Unauthorized(string error)
            => new(ResultStatus.Unauthorized, new List<string> { error });

        public static Result<T> Forbidden(string error)
            => new(ResultStatus.Forbidden, new List<string> { error });
    }
}