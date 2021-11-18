namespace Assistant.Bot.Core.Chat;

/// <summary>
/// Chat context wrapping the necessary information to handle the user request when passing it onto the handlers
/// </summary>
public interface IChatContext 
{
    /// <summary>
    /// The username of the sender, used to identify the request's origin during its processing
    /// </summary>
    string SenderUsername { get; }
}
