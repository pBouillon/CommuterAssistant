using MediatR;

using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Behaviour;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
        => _logger = logger;

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogInformation("Incoming request: {@Request}", request);
        return next();
    }
}
