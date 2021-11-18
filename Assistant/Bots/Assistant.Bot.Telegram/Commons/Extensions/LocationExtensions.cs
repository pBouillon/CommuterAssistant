using Assistant.Contracts.ValueObjects.Location;

using Telegram.Bot.Types;

namespace Assistant.Bot.Telegram.Commons.Extensions;

/// <summary>
/// <see cref="Location"/> extension methods
/// </summary>
public static class LocationExtensions
{
    /// <summary>
    /// Extract the coordinate of the <see cref="Location"/> property of a <see cref="Message"/>
    /// </summary>
    /// <param name="message">The message from which the coordonate will be extracted</param>
    /// <returns>The coordinate contained in the message as a <see cref="GeoCoordinate"/></returns>
    /// <remarks>
    /// The <see cref="GeoCoordinate"/> will be referring to latitude: 0, longitude: 0 if either the message or its
    /// location are null
    /// </remarks>
    public static GeoCoordinate GetGeoCoordinate(this Message? message)
        => message is null
            ? new(0, 0)
            : new(message.Location?.Latitude ?? 0, message.Location?.Longitude ?? 0);
}
