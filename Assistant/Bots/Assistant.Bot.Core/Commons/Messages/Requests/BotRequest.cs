using Assistant.Bot.Core.Chat;

using Destructurama.Attributed;

using MediatR;

namespace Assistant.Bot.Core.Commons.Messages.Requests;

/// <summary>
/// Request emitted by the user when messaging the <see cref="IAssistant"/>
/// </summary>
/// <typeparam name="TResponse">The type of the response to be returned</typeparam>
public abstract class BotRequest<TResponse> : IRequest<TResponse>
{
    /// <summary>
    /// The associated messaging context
    /// </summary>
    [NotLogged]
    public IChatContext Context { get; init; } = null!;
}
