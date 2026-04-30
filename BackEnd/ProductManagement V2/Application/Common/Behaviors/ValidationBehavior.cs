using FluentValidation;
using MediatR;
using ProductManagement_V2.Application.Common.Results;

namespace ProductManagement_V2.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Where(e => e is not null)
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToList();

            if (!errors.Any())
                return await next();

            var responseType = typeof(TResponse);

            if (responseType == typeof(Result))
            {
                return (TResponse)(object)Result.Validation(errors);
            }

            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = responseType.GetGenericArguments()[0];
                var validationMethod = responseType.GetMethod(
                    nameof(Result<object>.Validation),
                    new[] { typeof(List<string>) });

                if (validationMethod is not null)
                {
                    var validationResult = validationMethod.Invoke(null, new object[] { errors });
                    return (TResponse)validationResult!;
                }
            }

            throw new InvalidOperationException(
                $"ValidationBehavior expected response type '{nameof(Result)}' or '{nameof(Result<object>)}', but got '{responseType.Name}'.");
        }
    }
}