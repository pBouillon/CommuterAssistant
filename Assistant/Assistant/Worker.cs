using Assistant.Bot.Core;

namespace Assistant;

/// <summary>
/// Assistant's entry point
/// </summary>
public class Worker : BackgroundService
{
    /// <summary>
    /// The assistant to be used
    /// </summary>
    private readonly IAssistant _assistant;
    
    /// <summary>
    /// The worker's logger
    /// </summary>
    private readonly ILogger<Worker> _logger;

    /// <summary>
    /// Initialize a new instance of the worker
    /// </summary>
    /// <param name="assistant">The assistant to be used</param>
    /// <param name="logger">The worker's logger</param>
    public Worker(IAssistant assistant, ILogger<Worker> logger)
        => (_assistant, _logger) = (assistant, logger);

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _assistant.StartReceiving(stoppingToken);
        _logger.LogInformation("{AssistantType} has been started", _assistant.GetType().FullName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1_000, stoppingToken);
        }
    }
}
