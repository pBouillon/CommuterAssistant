using Assistant.Bot.Core.Chat;

using Telegram.Bot;
using Telegram.Bot.Types;

namespace Assistant.TelegramBot.Chat;

/// <summary>
/// <a href="https://telegram.org/">Telegram</a>-specific <see cref="IChatContext"/>
/// </summary>
public class TelegramContext : IChatContext
{
    /// <summary>
    /// The message sent by the client
    /// </summary>
    public Message Message { get; init; } = null!;

    /// <summary>
    /// The client that has received the <see cref="Message"/>
    /// </summary>
    public ITelegramBotClient Client { get; init; } = null!;

    /// <summary>
    /// The <a href="https://telegram.org/">Telegram</a> <see cref="User"/> associated to the sender
    /// </summary>
    public User Sender
        => Message.From;
    
    /// <inheritdoc />
    public string SenderUsername
        => Sender.Username;
}
