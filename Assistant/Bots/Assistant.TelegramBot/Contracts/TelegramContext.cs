using Assistant.Contracts.Chat;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Contracts;

public class TelegramContext : IChatContext
{
    public Message Message { get; init; } = null!;

    public ITelegramBotClient Client { get; init; } = null!;

    public User Sender
        => Message.From;
}
