namespace Assistant.Bot.Core.Chat;

/// <summary>
/// Commands to update the user's point of interest location known by the assistant
/// </summary>
public enum LocationUpdateCommand
{
    /// <summary>
    /// Command qualifying the user's intention to update their home's location
    /// </summary>
    Home,

    /// <summary>
    /// Command qualifying the user's intention to update their workplace's location
    /// </summary>
    Work,
}
