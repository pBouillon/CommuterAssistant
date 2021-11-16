using Assistant.Bot.Core.Commons.Exceptions;

namespace Assistant.TelegramBot.Commons.Exceptions;

public class MalformedCommandException : AssistantException
{
    public override string FriendlyErrorMessage => @"
The latitude and longitude should be provided along with the command as:
→ /home longitude, latitude
→ /work longitude, latitude

Examples:
→ /home 1.23, 4.56
→ /work 1.23, 4.56
";

    public MalformedCommandException()
    {
    }

    public MalformedCommandException(string message)
        : base(message)
    {
    }

    public MalformedCommandException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
