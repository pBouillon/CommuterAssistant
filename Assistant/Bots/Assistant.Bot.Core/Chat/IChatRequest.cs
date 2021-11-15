namespace Assistant.Bot.Core.Chat;

public interface IChatRequest<TContext> where TContext : IChatContext, new()
{
    TContext ChatContext { get; }
}
