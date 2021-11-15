using Assistant.Contracts.Bot;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace Assistant.TelegramBot;

public static class DependencencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(builder =>
        {
            var botConfiguration = builder.GetRequiredService<BotConfiguration>();
            return new TelegramBotClient(botConfiguration.ApiKey);
        });

        return services;
    }
}
