using Assistant.Contracts.Entities;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Bot.Core.Services;

/// <summary>
/// Assistant's database context
/// </summary>
public interface IApplicationContext
{
    /// <summary>
    /// The persisted users coordinates
    /// </summary>
    DbSet<Coordinate> Coordinates { get; }

    /// <summary>
    /// The persisted users
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Commit the changes to the database
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    int SaveChanges();

    /// <summary>
    /// Commit the changes to the database
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

