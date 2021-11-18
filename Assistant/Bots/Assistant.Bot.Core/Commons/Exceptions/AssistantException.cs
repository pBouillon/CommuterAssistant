namespace Assistant.Bot.Core.Commons.Exceptions;

/// <summary>
/// Assistant exception base to qualify the exceptions occuring within the assistant's logic
/// </summary>
public abstract class AssistantException : Exception
{
    /// <summary>
    /// A friendly error message that can be sent back to the client
    /// </summary>
    public abstract string FriendlyErrorMessage { get; }

    /// <summary>
    /// Initialize a new instance of the <see cref="AssistantException"/> class
    /// </summary>
    public AssistantException()
    {
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="AssistantException"/> class with a specified error message
    /// </summary>
    /// <param name="message">The error message</param>
    public AssistantException(string? message)
        : base(message)
    {
    }
    /// <summary>
    /// Initialize a new instance of the <see cref="AssistantException"/> class with a specified error message and a
    /// reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="inner">The exception causing this one</param>
    public AssistantException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
