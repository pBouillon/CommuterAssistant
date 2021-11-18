using Assistant.Bot.Core.Commons.Configuration;
using Assistant.Bot.Core.Services;
using Assistant.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assistant.Infrastructure;

/// <summary>
/// Dependency injection helper methods
/// </summary>
public static class DepencencyInjection
{
    /// <summary>
    /// Inject the services required by the assistant's infrastructure
    /// </summary>
    /// <param name="services">The dependency injection container</param>
    /// <returns>The dependency injection container</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var botConfiguration = configuration
            .GetSection(nameof(BotConfiguration))
            .Get<BotConfiguration>();

        services
            .AddPersistence()
            .AddSingleton(botConfiguration);

        return services;
    }

    /// <summary>
    /// Inject the persistence services
    /// </summary>
    /// <param name="services">The dependency injection container</param>
    /// <returns>The dependency injection container</returns>
    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        keepAliveConnection.Open();

        services
            .AddDbContext<ApplicationContext>(options => options.UseSqlite(keepAliveConnection))
            .AddTransient<IApplicationContext, ApplicationContext>();

        return services;
    }
}
