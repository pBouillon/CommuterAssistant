using Assistant.Bot.Core;
using Assistant.Bot.Core.Commons.Configuration;
using Assistant.TelegramBot.Handlers.Chat;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace Assistant.Bot.Telegram;

/// <summary>
/// The <a href="https://telegram.org/">Telegram</a> bot client
/// </summary>
public class Bot : IAssistant
{
    /// <summary>
    /// The bot configuration
    /// </summary>
    private readonly BotConfiguration _botConfiguration;

    /// <summary>
    /// The handlers to be used when the <see cref="_telegramClient"/> is emitting an event
    /// </summary>
    private readonly ChatEventsHandler _chatEventsHandler;

    /// <summary>
    /// The client interface from which the bot will be able to interact with the
    /// <a href="https://telegram.org/">Telegram</a> API
    /// </summary>
    private readonly ITelegramBotClient _telegramClient;

    /// <summary>
    /// The bot's logger
    /// </summary>
    private readonly ILogger<Bot> _logger;

    /// <summary>
    /// Create a new instance of the bot
    /// </summary>
    /// <param name="botConfiguration">The bot configuration</param>
    /// <param name="chatEventsHandler">The handlers to be used when the <see cref="_telegramClient"/> is emitting an event</param>
    /// <param name="telegramClient">
    /// The client interface from which the bot will be able to interact with the
    /// <a href="https://telegram.org/">Telegram</a> API
    /// </param>
    /// <param name="logger">The bot's logger</param>
    public Bot(
        BotConfiguration botConfiguration, ChatEventsHandler chatEventsHandler,
        ITelegramBotClient telegramClient, ILogger<Bot> logger)
    {
        _botConfiguration = botConfiguration;
        _chatEventsHandler = chatEventsHandler;
        _telegramClient = telegramClient;
        _logger = logger;
    }

    /// <inheritdoc />
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
