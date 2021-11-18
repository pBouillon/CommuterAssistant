namespace Assistant.Bot.Core.Chat;

/// <summary>
/// MediatR request emitted due to a user message
/// </summary>
/// <typeparam name="TContext">
/// The associated <see cref="IChatContext"/>, given the messaging application used
/// </typeparam>
public interface IChatRequest<TContext>
    where TContext : IChatContext, new()
{
    /// <summary>
    /// The associated context to handle the request
    /// </summary>
    TContext ChatContext { get; }
}
