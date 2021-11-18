using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

/// <summary>
/// Exception to be thrown when a message that cannot be handled by the assistant is received
/// </summary>
public class InvalidMessageException : AssistantException
{
    /// <inheritdoc />
    public override string FriendlyErrorMessage => @"
This message cannot be processed.

To set your points of interest, please use:
→ /home latitude, longitude
→ /work latitude, longitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    /// <inheritdoc />
    public InvalidMessageException()
    {
    }

    /// <inheritdoc />
    public InvalidMessageException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public InvalidMessageException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
