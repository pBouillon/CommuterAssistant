using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

public class InvalidMessageException : AssistantException
{
    public override string FriendlyErrorMessage => @"
This message cannot be processed.

To set your points of interest, please use:
→ /home latitude, longitude
→ /work latitude, longitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    public InvalidMessageException()
    {
    }

    public InvalidMessageException(string message)
        : base(message)
    {
    }

    public InvalidMessageException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
