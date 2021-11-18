using Assistant.Bot.Core.Commons.Messages.Requests;

using MediatR;

using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace Assistant.Bot.Core.Commons.Behaviour;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BotRequest<TResponse>, IRequest<TResponse>
{
    /// <summary>
    /// Treshold, in milliseconds, above which the request will be considered as long running
    /// </summary>
    private const int LongRunningRequestThreshold = 1_000;

    /// <summary>
    /// The request's logger
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// The internal stopwatch used to measure the execution's time
    /// </summary>
    private readonly Stopwatch _timer;

    /// <summary>
    /// Behaviour's constructor
    /// </summary>
    /// <param name="logger">The request's logger</param>
    public PerformanceBehaviour(ILogger<TRequest> logger)
        => (_logger, _timer) = (logger, new Stopwatch());

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds > LongRunningRequestThreshold)
        {
            _logger.LogWarning(
                "Long running request {@Request} from {User} ({Milliseconds} milliseconds)",
                request, request.Context.SenderUsername, elapsedMilliseconds);
        }

        return response;
    }
}
