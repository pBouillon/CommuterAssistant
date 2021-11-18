namespace Assistant.Bot.Core;

/// <summary>
/// The assistant that can reply to the messages it receives
/// </summary>
public interface IAssistant
{
    /// <summary>
    /// Start the assistant's listening to incoming requests and messages as a fire and forget way
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    void StartReceiving(CancellationToken cancellationToken);
}
