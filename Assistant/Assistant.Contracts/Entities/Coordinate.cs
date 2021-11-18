using Assistant.Contracts.Enums;

namespace Assistant.Contracts.Entities;

/// <summary>
/// A user-related coordinate of one of his point of interest
/// </summary>
public class Coordinate
{
    /// <summary>
    /// The coordinate id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user it refers to
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// The point of interest type
    /// </summary>
    public CoordinateType Type { get; set; }

    /// <summary>
    /// The location's latitude
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// The location's longitude
    /// </summary>
    public double Longitude { get; set; }
}
