using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.API.Configurations.Mediator;

internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly TimeProvider _timeProvider;
    private const string Prefix = nameof(LoggingBehavior<TRequest, TResponse>);

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("[{Prefix}] Handling request: {RequestType};", Prefix, typeof(TRequest).Name);

        var startTime = _timeProvider.GetTimestamp();
        var response = await next();

        var timeTaken = _timeProvider.GetElapsedTime(startTime).TotalSeconds;
        var elapsedTime = Math.Round(timeTaken, 2);

        if (timeTaken > 3)
        {
            _logger.LogWarning("[{Prefix}] Request {RequestType} took {ElapsedTime}s;",
                Prefix, typeof(TRequest).Name, elapsedTime);
        }

        _logger.LogDebug("[{Prefix}] Handled request: {Request}; Response Data: {Response};",
            Prefix, typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}