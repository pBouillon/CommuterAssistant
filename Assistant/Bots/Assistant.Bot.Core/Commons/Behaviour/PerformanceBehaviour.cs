using Assistant.Bot.Core.Messages;

using MediatR;

using Microsoft.Extensions.Logging;

using System.Diagnostics;

namespace Assistant.Bot.Core.Commons.Behaviour;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BotRequest<TResponse>, IRequest<TResponse>
{
    private const int LongRunningRequestThreshold = 500;

    private readonly ILogger<TRequest> _logger;

    private readonly Stopwatch _timer;

    public PerformanceBehaviour(ILogger<TRequest> logger)
        => (_logger, _timer) = (logger, new Stopwatch());

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
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
