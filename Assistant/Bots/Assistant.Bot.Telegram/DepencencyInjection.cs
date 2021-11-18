using Assistant.Bot.Core;
using Assistant.Bot.Core.Commons.Configuration;
using Assistant.TelegramBot.Handlers.Chat;

using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

namespace Assistant.Bot.Telegram;

/// <summary>
/// Dependency injection helper methods
/// </summary>
public static class DepencencyInjection
{
    /// <summary>
    /// Inject the services required by the bot
    /// </summary>
    /// <param name="services">The dependency injection container</param>
    /// <returns>The dependency injection container</returns>
    public static IServiceCollection AddTelegramBot(this IServiceCollection services)
        => services
            .AddSingleton<ITelegramBotClient>(builder =>
            {
                var botConfiguration = builder.GetRequiredService<BotConfiguration>();
                return new TelegramBotClient(botConfiguration.ApiKey);
            })
            .AddSingleton<ChatEventsHandler>()
            .AddSingleton<IAssistant, Bot>();
}
