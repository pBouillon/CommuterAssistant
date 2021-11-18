namespace Assistant.Contracts.ValueObjects.Location;

/// <summary>
/// Represent a carthesian coordinate
/// </summary>
/// <param name="Latitude">The coordinate's latitude</param>
/// <param name="Longitude">The coordinate's latitude</param>
public record GeoCoordinate(double Latitude, double Longitude)
{
    /// <summary>
    /// Estimate the distance between two <see cref="GeoCoordinate"/> according to
    /// <a href="https://stackoverflow.com/a/60899418/6152689">this StackOverflow answer</a>.
    /// </summary>
    /// <param name="other">The targeted coordinate</param>
    /// <returns>The distance to the other coordinate, in meters</returns>
    public double GetDistanceTo(GeoCoordinate other)
    {
        var d1 = Latitude * (Math.PI / 180.0);
        var num1 = Longitude * (Math.PI / 180.0);

        var d2 = other.Latitude * (Math.PI / 180.0);
        var num2 = other.Longitude * (Math.PI / 180.0) - num1;

        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0)
            + Math.Cos(d1) * Math.Cos(d2)
            * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }
}
