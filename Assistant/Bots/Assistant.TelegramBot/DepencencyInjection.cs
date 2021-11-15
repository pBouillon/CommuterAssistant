using Assistant.Contracts.Bot;
using Assistant.Infrastructure;
using Assistant.TelegramBot.Handlers.Chat;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace Assistant.TelegramBot;

public static class DepencencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(builder =>
        {
            var botConfiguration = builder.GetRequiredService<BotConfiguration>();
            return new TelegramBotClient(botConfiguration.ApiKey);
        });

        services
            .AddSingleton<ChatEventsHandler>()
            .AddSingleton<IAssistant, Bot>();

        return services;
    }
}
