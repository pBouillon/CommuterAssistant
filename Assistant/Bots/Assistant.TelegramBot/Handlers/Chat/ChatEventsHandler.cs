using Assistant.Bot.Core.Chat;
using Assistant.Bot.Core.Commons.Configuration;
using Assistant.Bot.Core.Commons.Exceptions;
using Assistant.Bot.Core.Messages;
using Assistant.Contracts.ValueObjects.Location;
using Assistant.TelegramBot.Chat;
using Assistant.TelegramBot.Commons.Exceptions;
using Assistant.TelegramBot.Commons.Extensions;

using MediatR;

using Microsoft.Extensions.Logging;

using System.Globalization;

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

        if (chatContext.Message is null)
        {
            _logger.LogWarning("Received a message of type {Type} with en empty Message payload: {@Message}", update.Type, update);
            return;
        }

        var isUserAllowed = _botConfiguration.AllowedUsers.Contains(chatContext.SenderUsername);

        try
        {
            var handleIncomingMessageTask = (isUserAllowed, chatContext.Message.Type) switch
            {
                (true, MessageType.Text) => HandleTextMessageAsync(chatContext, cancellationToken),
                (true, MessageType.Location) => HandleLocationMessageAsync(chatContext, cancellationToken),
                (true, _) => HandleOtherMessageType(chatContext),
                _ => HandleUnauthorizedUserMessageAsync(chatContext, cancellationToken),
            };

            await handleIncomingMessageTask;
        }
        catch (AssistantException exception)
        {
            _logger.LogError(exception, "An error occurred while processing the message of {Sender}", chatContext.SenderUsername);

            await chatContext.Client.ReplyToAsync(
                chatContext.Message,
                exception.FriendlyErrorMessage,
                cancellationToken);
        }
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

        var response = message.StartsWith('/')
            ? await HandleCommandAsync(context, cancellationToken)
            : throw new InvalidMessageException($"Unhandled message: {message}");

        return await context.Client.ReplyToAsync(context.Message, response, cancellationToken);
    }

    private Task<string> HandleCommandAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        var (command, coordinate) = ExtractPayloadFrom(context.Message.Text);

        return command switch
        {
            LocationUpdateCommand.Home => _mediator.Send(
                new SetHomeLocationRequest { Context = context, Coordinate = coordinate, },
                cancellationToken),
            LocationUpdateCommand.Work => _mediator.Send(
                new SetWorkplaceLocationRequest { Context = context, Coordinate = coordinate, },
                cancellationToken),
            _ => throw new InvalidCommandException($"Unhandled command: {command}"),
        };
    }

    private (LocationUpdateCommand command, GeoCoordinate coordinate) ExtractPayloadFrom(string message)
    {
        var rawCommand = message
            .Split(' ')
            .First();

        var isKnownCommand = Enum.TryParse<LocationUpdateCommand>(rawCommand[1..], true, out var command);
        if (!isKnownCommand) throw new InvalidCommandException($"Unknown command: {rawCommand}");

        var rawCoordinates = message
            .Remove(0, rawCommand.Length)
            .Split(',')
            .ToArray();

        if (rawCoordinates.Length != 2) throw new MalformedCommandException();

        try
        {
            var coordinates = rawCoordinates
                .Select(coordinate => double.Parse(coordinate, NumberStyles.Any, CultureInfo.InvariantCulture))
                .ToArray();

            return (command, new()
            {
                Latitude = coordinates[0],
                Longitude = coordinates[1],
            });
        }
        catch (Exception ex)
        {
            throw new MalformedCommandException("An error occurred while converting the payload to a GeoCoordinate", ex);
        }
    }

    private async Task<Message> HandleLocationMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {Sender}'s location", context.SenderUsername);

        var nextDepartureInsights = await _mediator.Send(new GetNextDepartureRequest(), cancellationToken);

        return await context.Client.ReplyToAsync(context.Message, nextDepartureInsights, cancellationToken);
    }

    private Task<Message> HandleOtherMessageType(TelegramContext context)
        => throw new InvalidMessageTypeException(
            $"Received unhandled message type ({context.Message.Type}) received from {context.SenderUsername}");
}
