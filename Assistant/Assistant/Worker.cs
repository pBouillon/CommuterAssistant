using Assistant.Bot.Core;

namespace Assistant;

public class Worker : BackgroundService
{
    private readonly IAssistant _assistant;
    
    private readonly ILogger<Worker> _logger;
    
    public Worker(IAssistant assistant, ILogger<Worker> logger)
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
