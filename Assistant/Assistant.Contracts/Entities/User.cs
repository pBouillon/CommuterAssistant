namespace Assistant.Contracts.Entities;

/// <summary>
/// The user that can interact with the assistant
/// </summary>
public class User
{
    /// <summary>
    /// The user id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user name by which he will be identified when sending a message
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// A collection of the user's points of interest
    /// </summary>
    public IList<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
}

