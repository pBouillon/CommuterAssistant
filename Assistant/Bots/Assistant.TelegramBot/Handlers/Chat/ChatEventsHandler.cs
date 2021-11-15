using Assistant.Bot.Core.Commons.Configuration;
using Assistant.Bot.Core.Messages;
using Assistant.TelegramBot.Chat;
using Assistant.TelegramBot.Commons.Extensions;

using MediatR;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Assistant.TelegramBot.Handlers.Chat;

public class ChatEventsHandler
{
    private readonly BotConfiguration _botConfiguration;

    private readonly ILogger<ChatEventsHandler> _logger;

    private readonly IMediator _mediator;

    public ChatEventsHandler(BotConfiguration botConfiguration, ILogger<ChatEventsHandler> logger, IMediator mediator)
        => (_botConfiguration, _logger, _mediator) = (botConfiguration, logger, mediator);

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

        var isUserAllowed = _botConfiguration.AllowedUsers.Contains(chatContext.SenderUsername);

        var handleIncomingMessageTask = (isUserAllowed, chatContext.Message.Type) switch
        {
            (true, MessageType.Text) => HandleTextMessageAsync(chatContext, cancellationToken),
            (true, MessageType.Location) => HandleLocationMessageAsync(chatContext, cancellationToken),
            (true, _) => HandleOtherMessageTypeAsync(chatContext, cancellationToken),
            _ => HandleUnauthorizedUserMessageAsync(chatContext, cancellationToken),
        };

        await handleIncomingMessageTask;
    }

    private Task<Message> HandleUnauthorizedUserMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Message received from the unauthorized sender {Sender}", context.SenderUsername);

        return context.Client.ReplyToAsync(
            context.Message,
            "You do not belong to the authorized users, your request has been discarded.",
            cancellationToken);
    }

    private async Task<Message> HandleTextMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        var message = context.Message.Text;
            
        _logger.LogInformation("Received {Content} from {Sender}", message, context.SenderUsername);

        var updatedLocation = message switch
        {
            var home when home.StartsWith("/home") => await _mediator.Send(
                new SetHomeLocationRequest { Context = context }, cancellationToken),
            var workplace when workplace.StartsWith("/work") => await _mediator.Send(
                new SetWorkplaceLocationRequest { Context = context }, cancellationToken),
            _ => string.Join("\n",
                    "This message cannot be processed.",
                    "",
                    "To set your points of interest, please use:",
                    "/home longitude, latitude",
                    "/work longitude, latitude")
        };

        return await context.Client.ReplyToAsync(context.Message, updatedLocation, cancellationToken);
    }

    private async Task<Message> HandleLocationMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {Sender}'s location", context.SenderUsername);

        var nextDepartureInsights = await _mediator.Send(new GetNextDepartureRequest(), cancellationToken);

        return await context.Client.ReplyToAsync(context.Message, nextDepartureInsights, cancellationToken);
    }

    private Task<Message> HandleOtherMessageTypeAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Received unhandled message type ({Type}) from {Sender}", context.Message.Type, context.SenderUsername);

        return context.Client.ReplyToAsync(
            context.Message,
            "You do not belong to the authorized users, your request has been discarded.",
            cancellationToken);
    }
}
