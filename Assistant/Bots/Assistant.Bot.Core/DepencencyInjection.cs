using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Assistant.Bot.Core;

public static class DepencencyInjection
{
    public static IServiceCollection AddBotCore(this IServiceCollection services)
        => services.AddMediatR(Assembly.GetExecutingAssembly());
}
