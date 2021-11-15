using Assistant.Bot.Core;
using Assistant.Bot.Core.Commons.Configuration;
using Assistant.TelegramBot.Handlers.Chat;

using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace Assistant.TelegramBot;

public static class DepencencyInjection
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
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
