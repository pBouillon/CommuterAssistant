using Assistant.Contracts.ValueObjects.Location;

namespace Assistant.Bot.Core.Commons.Messages;

public class LocationUpdateRequest<TResponse> : BotRequest<TResponse>
{
    public GeoCoordinate Coordinate { get; set; } = GeoCoordinate.Default;
}
