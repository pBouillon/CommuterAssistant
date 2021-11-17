using Assistant.Contracts.ValueObjects.Location;

namespace Assistant.Bot.Core.Commons.Messages;

public class BotLocationRequest<TResponse> : BotRequest<TResponse>
{
    public GeoCoordinate Location { get; set; } = GeoCoordinate.Default;
}
