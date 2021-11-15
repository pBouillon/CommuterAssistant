using Assistant.Bot.Core.Services;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Infrastructure.Persistence;

public class ApplicationContext : DbContext, IApplicationContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
    {
    }
}
