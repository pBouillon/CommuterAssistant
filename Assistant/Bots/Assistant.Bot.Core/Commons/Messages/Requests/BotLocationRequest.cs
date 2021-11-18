using Assistant.Contracts.ValueObjects.Location;

namespace Assistant.Bot.Core.Commons.Messages.Requests;

/// <summary>
/// Request related to the user's location
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public class BotLocationRequest<TResponse> : BotRequest<TResponse>
{
    /// <summary>
    /// Location transmitted by the user
    /// </summary>
    public GeoCoordinate Location { get; set; } = GeoCoordinate.Default;
}
