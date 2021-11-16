using Assistant.Contracts.Entities;
using Assistant.Contracts.ValueObjects.Location;

namespace Assistant.Contracts.Extentions;

public static class CoordinateExtensions
{
    public static GeoCoordinate AsGeoCoordinate(this Coordinate coordinate)
        => new GeoCoordinate
        {
            Latitude = coordinate.Latitude,
            Longitude = coordinate.Longitude
        };
}

