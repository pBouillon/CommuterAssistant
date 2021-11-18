using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

/// <summary>
/// Exception to be thrown when an unknown command is received
/// </summary>
public class InvalidCommandException : AssistantException
{
    /// <inheritdoc />
    public override string FriendlyErrorMessage => @"
This is not a known command.

To set your points of interest, please use:
→ /home latitude, longitude
→ /work latitude, longitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    /// <inheritdoc />
    public InvalidCommandException()
    {
    }

    /// <inheritdoc />
    public InvalidCommandException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public InvalidCommandException(string message, Exception inner)
        : base(message, inner)
    {
    }
}