using Assistant.Contracts.Enums;

namespace Assistant.Contracts.Entities;

public class Coordinate
{
    public Guid Id { get; set; }

    public User User { get; set; } = null!;

    public CoordinateType Type { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }
}
