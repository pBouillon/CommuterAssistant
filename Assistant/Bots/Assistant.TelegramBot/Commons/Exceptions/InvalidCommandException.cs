using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

public class InvalidCommandException : AssistantException
{
    public override string FriendlyErrorMessage => @"
This is not a known command.

To set your points of interest, please use:
→ /home longitude, latitude
→ /work longitude, latitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    public InvalidCommandException()
    {
    }

    public InvalidCommandException(string message)
        : base(message)
    {
    }

    public InvalidCommandException(string message, Exception inner)
        : base(message, inner)
    {
    }
}