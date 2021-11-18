using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

/// <summary>
/// Exception to be thrown when a valid command is received but its format does not match the expected one
/// </summary>
public class MalformedCommandException : AssistantException
{
    /// <inheritdoc />
    public override string FriendlyErrorMessage => @"
The latitude and longitude should be provided along with the command as:
→ /home latitude, longitude
→ /work latitude, longitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    /// <inheritdoc />
    public MalformedCommandException()
    {
    }

    /// <inheritdoc />
    public MalformedCommandException(string message)
        : base(message)
    {
    }

    /// <inheritdoc />
    public MalformedCommandException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
