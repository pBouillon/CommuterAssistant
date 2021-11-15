using Assistant.Contracts.Bot;

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

        services.AddSingleton(botConfiguration);

        return services;
    }
}
