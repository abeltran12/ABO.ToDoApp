using ABO.ToDoApp.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace ABO.ToDoApp.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = _validators
            .SelectMany(validator => validator.Validate(context).Errors)
            .Where(failure => failure != null)
            .GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.ToArray());

        if (validationFailures.Any())
            throw new ValidationAppException(validationFailures);
           
        return await next();
    }
}
