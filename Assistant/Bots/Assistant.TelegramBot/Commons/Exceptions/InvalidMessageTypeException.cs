using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

public class InvalidMessageTypeException : AssistantException
{
    public override string FriendlyErrorMessage => @"
This message cannot be processed.

To set your points of interest, please use:
→ /home longitude, latitude
→ /work longitude, latitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    public InvalidMessageTypeException()
    {
    }

    public InvalidMessageTypeException(string message)
        : base(message)
    {
    }

    public InvalidMessageTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}