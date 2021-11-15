using Assistant.Infrastructure;

namespace Assistant;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    
    private readonly IAssistant _assistant;

    public Worker(IAssistant assistant,  ILogger<Worker> logger)
        => (_assistant, _logger) = (assistant, logger);

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
