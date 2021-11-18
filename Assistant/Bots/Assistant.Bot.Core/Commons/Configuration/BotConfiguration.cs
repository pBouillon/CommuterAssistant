namespace Assistant.Bot.Core.Commons.Configuration;

/// <summary>
/// POCO wrapping the <see cref="IAssistant"/> bot's configuration
/// </summary>
public class BotConfiguration
{
    /// <summary>
    /// The API key to be used for the bot messaging's client
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The users allowed to interact with the assistant, by their usernames
    /// </summary>
    public List<string> AllowedUsers { get; set; } = new();
}
