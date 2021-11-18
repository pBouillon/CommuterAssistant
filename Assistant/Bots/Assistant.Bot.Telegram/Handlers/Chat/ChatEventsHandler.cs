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

/// <summary>
/// Chat event handler, providing method to handle incoming messages and errors
/// </summary>
public class ChatEventsHandler
{
    /// <summary>
    /// The bot configuration
    /// </summary>
    private readonly BotConfiguration _botConfiguration;

    /// <summary>
    /// The handler's logger
    /// </summary>
    private readonly ILogger<ChatEventsHandler> _logger;

    /// <summary>
    /// Mediator to forward the client's requests internally
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Initialize a new instance of the handler
    /// </summary>
    /// <param name="botConfiguration">The bot configuration</param>
    /// <param name="logger">The handler's logger</param>
    /// <param name="mediator">Mediator to forward the client's requests internally</param>
    public ChatEventsHandler(BotConfiguration botConfiguration, ILogger<ChatEventsHandler> logger, IMediator mediator)
        => (_botConfiguration, _logger, _mediator) = (botConfiguration, logger, mediator);

    /// <summary>
    /// The handlers to be used to handle chat updates and errors
    /// </summary>
    public (Func<ITelegramBotClient, Update, CancellationToken, Task>,
            Func<ITelegramBotClient, Exception, CancellationToken, Task>) Handlers
        => (HandleUpdateAsync, HandleErrorAsync);

    /// <summary>
    /// Handle an error that has occured
    /// </summary>
    /// <param name="client">The client in error</param>
    /// <param name="exception">The exception that has been thrown</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A completed task after handling the exception</returns>
    private Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected exception occurred");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handle an incoming message
    /// </summary>
    /// <param name="client">The client that has received the message</param>
    /// <param name="update">The update's content</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>An awaitable task</returns>
    private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var chatContext = new TelegramContext { Client = client, Message = update.Message };

        if (chatContext.Message is null)
        {
            _logger.LogWarning(
                "Received a message of type {Type} with en empty Message payload: {@Message}",
                update.Type, update);

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
            _logger.LogError(
                exception,
                "An error occurred while processing the message of {Sender}", chatContext.SenderUsername);

            await chatContext.Client.ReplyToAsync(
                chatContext.Message,
                exception.FriendlyErrorMessage,
                cancellationToken);
        }
    }

    private Task<Message> HandleUnauthorizedUserMessageAsync(
        TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Message received from the unauthorized sender {Sender}", context.SenderUsername);

        return context.Client.ReplyToAsync(
            context.Message,
            "You do not belong to the authorized users, your request has been discarded.",
            cancellationToken);
    }

    /// <summary>
    /// Process an incoming text message
    /// </summary>
    /// <param name="context">The <see cref="IChatContext"/> in which the message has been received</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that is wrapping the response sent back to the user</returns>
    /// <exception cref="InvalidMessageException">The received message is not a command</exception>
    private async Task<Message> HandleTextMessageAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        var message = context.Message.Text;
        _logger.LogInformation("Received {Content} from {Sender}", message, context.SenderUsername);

        var response = message.StartsWith('/')
            ? await HandleCommandAsync(context, cancellationToken)
            : throw new InvalidMessageException($"Unhandled message: {message}");

        return await context.Client.ReplyToAsync(context.Message, response, cancellationToken);
    }

    /// <summary>
    /// Process an incoming command
    /// </summary>
    /// <param name="context">The <see cref="IChatContext"/> in which the message has been received</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that is wrapping the response's content to be sent back to the user</returns>
    /// <exception cref="InvalidCommandException">The command is not known nor handled by the system</exception>
    private Task<string> HandleCommandAsync(TelegramContext context, CancellationToken cancellationToken)
    {
        var (command, coordinate) = ExtractLocationUpdatePayloadFrom(context.Message.Text);

        return command switch
        {
            LocationUpdateCommand.Home => _mediator.Send(
                new SetHomeLocationRequest { Context = context, Location = coordinate, },
                cancellationToken),
            LocationUpdateCommand.Work => _mediator.Send(
                new SetWorkplaceLocationRequest { Context = context, Location = coordinate, },
                cancellationToken),
            _ => throw new InvalidCommandException($"Unhandled command: {command}"),
        };
    }

    /// <summary>
    /// Extract the payload required to process the command as a location update command
    /// </summary>
    /// <param name="message"></param>
    /// <returns>
    /// A tuple containing the type of the command emitted by the user along with the associated
    /// <see cref="GeoCoordinate"/>
    /// </returns>
    /// <exception cref="InvalidCommandException">
    /// The command does not match any item of <see cref="LocationUpdateCommand"/>
    /// </exception>
    /// <exception cref="MalformedCommandException">The command's payload is not correctly formed</exception>
    private (LocationUpdateCommand command, GeoCoordinate coordinate) ExtractLocationUpdatePayloadFrom(string message)
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

            return (command, new GeoCoordinate
            {
                Latitude = coordinates[0],
                Longitude = coordinates[1],
            });
        }
        catch (Exception ex)
        {
            throw new MalformedCommandException(
                $"An error occurred while converting the payload to a {nameof(GeoCoordinate)}", ex);
        }
    }

    /// <summary>
    /// Update the user provided point of interest's location
    /// </summary>
    /// <param name="context">The <see cref="IChatContext"/> in which the message has been received</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task that is wrapping the response sent back to the user</returns>
    private async Task<Message> HandleLocationMessageAsync(
        TelegramContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received {Sender}'s location", context.SenderUsername);

        var nextDepartureInsights = await _mediator.Send(
            new GetNextDepartureRequest
            {
                Context = context,
                Location = new GeoCoordinate
                {
                    Latitude = context.Message.Location.Latitude,
                    Longitude = context.Message.Location.Longitude,
                },
            }, 
            cancellationToken);

        return await context.Client.ReplyToAsync(context.Message, nextDepartureInsights, cancellationToken);
    }

    /// <summary>
    /// Handle a message of a type that cannot be processed by the assistant
    /// </summary>
    /// <param name="context">The <see cref="IChatContext"/> in which the message has been received</param>
    /// <returns>A task that is wrapping the response sent back to the user</returns>
    /// <exception cref="InvalidMessageTypeException">The </exception>
    private Task<Message> HandleOtherMessageType(TelegramContext context)
    {
        throw new InvalidMessageTypeException(
            $"Received unhandled message type ({context.Message.Type}) received from {context.SenderUsername}");
    }
}
