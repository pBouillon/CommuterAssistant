using Assistant.TelegramBot.Contracts;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Handlers.Chat;

public class ChatEventsHandler
{
    private readonly ILogger<ChatEventsHandler> _logger;

    public ChatEventsHandler(ILogger<ChatEventsHandler> logger)
        => (_logger) = (logger);

    public (Func<ITelegramBotClient, Update, CancellationToken, Task>, Func<ITelegramBotClient, Exception, CancellationToken, Task>) Handlers
        => (HandleUpdateAsync, HandleErrorAsync);

    private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected exception occurred");
        return Task.CompletedTask;
    }

    private Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Received a message of type {Message} from {Sender}",
            update.Message.Type, update.Message.From.Username);

        var chatContext = new TelegramContext
        {
            Client = client,
            Message = update.Message,
        };

        return Task.CompletedTask;
    }
}
