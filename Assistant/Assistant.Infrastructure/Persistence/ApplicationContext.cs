using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Infrastructure.Persistence;

/// <inheritdoc cref="IApplicationContext"/>
public class ApplicationContext : DbContext, IApplicationContext
{
    /// <inheritdoc />
    public DbSet<Coordinate> Coordinates { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<User> Users { get; set; } = null!;

    /// <inheritdoc cref="DbContext"/>
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
    }
}
