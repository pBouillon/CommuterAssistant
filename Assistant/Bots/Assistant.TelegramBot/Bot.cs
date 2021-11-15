using Assistant.Contracts.Bot;
using Assistant.Infrastructure;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot;

public class Bot : IAssistant
{
    private readonly BotConfiguration _botConfiguration;
 
    private readonly ITelegramBotClient _client;

    private readonly ILogger<Bot> _logger;

    public Bot(BotConfiguration botConfiguration, ITelegramBotClient client, ILogger<Bot> logger)
        => (_botConfiguration, _client, _logger) = (botConfiguration, client, logger);

    public void StartReceiving(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting the assistant with the following allowed users: {AllowedUsers}",
            _botConfiguration.AllowedUsers);

        var updateHandler = new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync);
        _client.StartReceiving(updateHandler, cancellationToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    private Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
