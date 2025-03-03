using BuildingBlocks.API.Models.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.API.Configurations.Mediator;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private const string Prefix = nameof(ValidationBehavior<TRequest, TResponse>);

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        _logger.LogDebug("[{Prefix}] Validating request: {RequestType};", Prefix, typeof(TRequest).Name);
        var context = new ValidationContext<TRequest>(request);

        var failures = await Validate(_validators, context, cancellationToken);
        if (failures.Count > 0)
        {
            _logger.LogWarning("[{Prefix}] Validation failed for {RequestType} with {FailureCount} errors;", Prefix,
                typeof(TRequest).Name, failures.Count);
            throw new ValidationException(failures);
        }

        _logger.LogDebug("[{Prefix}] Validation passed for {RequestType};", Prefix, typeof(TRequest).Name);
        return await next();
    }

    private static async Task<List<ValidationFailure>> Validate(
        IEnumerable<IValidator<TRequest>> validators,
        ValidationContext<TRequest> context,
        CancellationToken cancellationToken)
    {
        var validatorTasks = validators
            .Select(async x => await x.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validatorTasks);
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        return failures;
    }
}