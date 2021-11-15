namespace Assistant.Infrastructure;

public interface IAssistant
{
    void StartReceiving(CancellationToken cancellationToken);
}
