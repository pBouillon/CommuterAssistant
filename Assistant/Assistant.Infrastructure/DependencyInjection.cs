using Assistant.Bot.Core.Commons.Configuration;
using Assistant.Bot.Core.Services;
using Assistant.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assistant.Infrastructure;

public static class DepencencyInjection
{
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
