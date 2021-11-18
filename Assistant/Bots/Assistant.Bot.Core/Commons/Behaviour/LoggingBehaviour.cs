using MediatR;

using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Behaviour;

/// <summary>
/// Behaviour interecepting a <typeparamref name="TRequest"/> and logging it before executing its handle method
/// </summary>
/// <typeparam name="TRequest">The transiting request</typeparam>
/// <typeparam name="TResponse">The response of the transiting <typeparamref name="TRequest"/></typeparam>
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// The request's logger
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// Behaviour's constructor
    /// </summary>
    /// <param name="logger">The request's logger</param>
    public LoggingBehaviour(ILogger<TRequest> logger)
        => _logger = logger;

    /// <inheritdoc />
    public Task<TResponse> Handle(
        TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogTrace("Incoming request: {@Request}", request);
        return next();
    }
}
