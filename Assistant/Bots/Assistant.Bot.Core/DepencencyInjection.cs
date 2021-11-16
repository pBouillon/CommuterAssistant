using Assistant.Bot.Core.Commons.Behaviour;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Assistant.Bot.Core;

public static class DepencencyInjection
{
    public static IServiceCollection AddBotCore(this IServiceCollection services)
        => services.AddMediatR(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CreateUserProfileIfNeededBehaviour<,>));
}
