using Assistant.Contracts.Bot;
using Assistant.TelegramBot.Commons.Extensions;
using Assistant.TelegramBot.Contracts;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Assistant.TelegramBot.Handlers.Chat;

public class ChatEventsHandler
{
    private readonly BotConfiguration _botConfiguration;

    private readonly ILogger<ChatEventsHandler> _logger;

    public ChatEventsHandler(BotConfiguration botConfiguration, ILogger<ChatEventsHandler> logger)
        => (_botConfiguration, _logger) = (botConfiguration, logger);

    public (Func<ITelegramBotClient, Update, CancellationToken, Task>, Func<ITelegramBotClient, Exception, CancellationToken, Task>) Handlers
        => (HandleUpdateAsync, HandleErrorAsync);

    private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected exception occurred");
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var chatContext = new TelegramContext { Client = client, Message = update.Message };

        var isUserAllowed = _botConfiguration.AllowedUsers.Contains(chatContext.Sender.Username);

        var handleIncomingMessageTask = (isUserAllowed, chatContext.Message.Type) switch
        {
            (true, MessageType.Text) => HandleTextMessageAsync(chatContext, cancellationToken),
            (true, MessageType.Location) => HandleLocationMessageAsync(chatContext, cancellationToken),
            (true, _) => HandleOtherMessageTypeAsync(chatContext, cancellationToken),
            _ => HandleUnauthorizedUserMessageAsync(chatContext, cancellationToken),
        };

        await handleIncomingMessageTask;
    }

    private Task HandleUnauthorizedUserMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Message received from the unauthorized sender {Sender}",
            context.Sender.Username);

        return context.Client.ReplyToAsync(
            context.Message,
            "You do not belong to the authorized users, your request has been discarded.",
            cancellationToken);
    }

    private Task HandleTextMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Received {Content} from {Sender}",
            context.Message.Text, context.Sender.Username);

        return context.Client.ReplyToAsync(
            context.Message,
            "Text received.",
            cancellationToken);
    }

    private Task HandleLocationMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {Sender}'s location", context.Sender.Username);

        return context.Client.ReplyToAsync(
            context.Message,
            "Location received",
            cancellationToken);
    }

    private Task HandleOtherMessageTypeAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Received unhandled message type ({Type}) from {Sender}",
            context.Message.Type, context.Sender.Username);

        return context.Client.ReplyToAsync(
            context.Message,
            "You do not belong to the authorized users, your request has been discarded.",
            cancellationToken);
    }
}
