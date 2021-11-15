using Assistant.Bot.Core;
using Assistant.Bot.Core.Commons.Configuration;
using Assistant.TelegramBot.Handlers.Chat;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace Assistant.TelegramBot;

public class Bot : IAssistant
{
    private readonly BotConfiguration _botConfiguration;
 
    private readonly ChatEventsHandler _chatEventsHandler;

    private readonly ITelegramBotClient _telegramClient;

    private readonly ILogger<Bot> _logger;

    public Bot(
        BotConfiguration botConfiguration, ChatEventsHandler chatEventsHandler,
        ITelegramBotClient telegramClient, ILogger<Bot> logger)
    {
        _botConfiguration = botConfiguration;
        _chatEventsHandler = chatEventsHandler;
        _telegramClient = telegramClient;
        _logger = logger;
    }

    public void StartReceiving(CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting the assistant with the following allowed users: {AllowedUsers}",
            _botConfiguration.AllowedUsers);

        var (chatUpdateHandler, errorHandler) = _chatEventsHandler.Handlers;

        var updateHandler = new DefaultUpdateHandler(chatUpdateHandler, errorHandler);
        _telegramClient.StartReceiving(updateHandler, cancellationToken);
    }
}
