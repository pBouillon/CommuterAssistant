
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Commons.Extensions;

/// <summary>
/// <see cref="ITelegramBotClient"/> extension methods
/// </summary>
public static class ITelegramBotClientExtensions
{
    /// <summary>
    /// Reply to a user with the provided content
    /// </summary>
    /// <param name="telegramClient">The <see cref="ITelegramBotClient"/> that has received the message</param>
    /// <param name="message">The message to reply to</param>
    /// <param name="responseContent">The message to be sent as a reply</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The sent message</returns>
    public static Task<Message> ReplyToAsync(
            this ITelegramBotClient telegramClient, Message message,
            string responseContent, CancellationToken cancellationToken = default)
        => telegramClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: responseContent,
                cancellationToken: cancellationToken);
}
