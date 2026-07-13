using FluentValidation;
using MediatR;

namespace ShopApi.Application.Behaviors
{
    public class ValidationBehavior
<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators; //get validators of request(command)

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())   //valid
            {
                var context =
                    new ValidationContext<TRequest>(request);

                var results =
                    await Task.WhenAll(
                        _validators.Select(x =>
                            x.ValidateAsync(context, cancellationToken)));

                var failures =
                    results
                    .SelectMany(x => x.Errors)
                    .Where(x => x != null)
                    .ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }

            return await next(); // nextpipline behavior
        }
    }
}