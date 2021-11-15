
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Commons.Extensions;

public static class ITelegramBotClientExtensions
{
    public static Task<Message> ReplyToAsync(
            this ITelegramBotClient telegramClient, Message message,
            string responseContent, CancellationToken cancellationToken = default)
        => telegramClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: responseContent,
                cancellationToken: cancellationToken);
}
