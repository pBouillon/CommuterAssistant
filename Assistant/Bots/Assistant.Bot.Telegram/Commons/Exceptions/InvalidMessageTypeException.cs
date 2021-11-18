using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

/// <summary>
/// Exception to be thrown when a message of an invalid type is received
/// </summary>
public class InvalidMessageTypeException : AssistantException
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
    public InvalidMessageTypeException()
    {
    }

    /// <inheritdoc />
    public InvalidMessageTypeException(string message)
        : base(message)
    {
    }
    
    /// <inheritdoc />
    public InvalidMessageTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}