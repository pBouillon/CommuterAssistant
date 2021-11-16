namespace Assistant.Bot.Core.Commons.Exceptions
{
    public abstract class AssistantException : Exception
    {
        public abstract string FriendlyErrorMessage { get; }

        public AssistantException()
        {
        }

        public AssistantException(string message)
            : base(message)
        {
        }

        public AssistantException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
