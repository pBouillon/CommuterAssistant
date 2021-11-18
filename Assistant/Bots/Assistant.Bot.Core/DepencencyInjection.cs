using Assistant.Bot.Core.Commons.Behaviour;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Assistant.Bot.Core;

/// <summary>
/// Dependency injection helper methods
/// </summary>
public static class DepencencyInjection
{
    /// <summary>
    /// Inject the services required by the assistant
    /// </summary>
    /// <param name="services">The dependency injection container</param>
    /// <returns>The dependency injection container</returns>
    public static IServiceCollection AddBotCore(this IServiceCollection services)
        => services.AddMediatR(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CreateUserProfileIfNeededBehaviour<,>));
}
