using Assistant.Bot.Core.Chat;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Chat;

public class TelegramContext : IChatContext
{
    public Message Message { get; init; } = null!;

    public ITelegramBotClient Client { get; init; } = null!;

    public User Sender
        => Message.From;

    public string SenderUsername
        => Sender.Username;
}
