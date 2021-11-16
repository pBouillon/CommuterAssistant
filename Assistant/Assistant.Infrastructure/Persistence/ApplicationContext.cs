using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Infrastructure.Persistence;

public class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<Coordinate> Coordinates { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
    }
}
