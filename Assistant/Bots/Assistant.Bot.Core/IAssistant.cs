namespace Assistant.Bot.Core;

public interface IAssistant
{
    void StartReceiving(CancellationToken cancellationToken);
}
