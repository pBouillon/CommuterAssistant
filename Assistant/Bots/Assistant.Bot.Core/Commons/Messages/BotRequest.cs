using Assistant.Bot.Core.Chat;

using Destructurama.Attributed;

using MediatR;

namespace Assistant.Bot.Core.Commons.Messages;

public abstract class BotRequest<TResponse> : IRequest<TResponse>
{
    [NotLogged]
    public IChatContext Context { get; init; } = null!;
}
