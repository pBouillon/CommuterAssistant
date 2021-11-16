using Assistant.Contracts.Entities;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Bot.Core.Services
{
    public interface IApplicationContext
    {
        DbSet<Coordinate> Coordinates { get; }

        DbSet<User> Users { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
