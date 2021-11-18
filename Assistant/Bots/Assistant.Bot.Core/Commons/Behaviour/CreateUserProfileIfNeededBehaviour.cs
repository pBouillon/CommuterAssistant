using Assistant.Bot.Core.Commons.Messages.Requests;
using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Behaviour;

/// <summary>
/// Behaviour interecepting a <see cref="BotRequest{TResponse}"/> and creating the associated requester as a user if he
/// does not exist in the database
/// </summary>
/// <typeparam name="TRequest">The transiting <see cref="BotRequest{TResponse}"/></typeparam>
/// <typeparam name="TResponse">The response of the transiting <see cref="BotRequest{TResponse}"/></typeparam>
public class CreateUserProfileIfNeededBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BotRequest<TResponse>, IRequest<TResponse>
{
    /// <summary>
    /// The request's logger
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// The service provider used to create a scope and access a scoped instance of the
    /// <see cref="IApplicationContext"/>
    /// </summary>
    /// <remarks>
    /// To see why the <see cref="IApplicationContext"/> is not directly injected,
    /// see <a href="https://stackoverflow.com/a/48368934/6152689">this StackOverflow answer</a>
    /// </remarks>
    private readonly IServiceProvider _services;

    /// <summary>
    /// Behaviour's constructor
    /// </summary>
    /// <param name="logger">The request's logger</param>
    /// <param name="services">
    /// The service provider used to create a scope and access a scoped instance of the
    /// <see cref="IApplicationContext"/>
    /// </param>
    public CreateUserProfileIfNeededBehaviour(ILogger<TRequest> logger, IServiceProvider services)
        => (_logger, _services) = (logger, services);

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var username = request.Context.SenderUsername;

        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationContext>();

        var isUserPersisted = context.Users.Any(user => user.Name == username);

        if (!isUserPersisted)
        {
            var user = await CreateUser(context, username, cancellationToken);
            _logger.LogInformation("User {@User} did not exist and has been created", new { user.Id, user.Name });
        }

        return await next();
    }

    /// <summary>
    /// Create a new user with the provided username
    /// </summary>
    /// <param name="context">The <see cref="IApplicationContext"/></param>
    /// <param name="username">The name of the user to create</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The created user</returns>
    private async Task<User> CreateUser(IApplicationContext context, string username, CancellationToken cancellationToken)
    {
        var user = new User { Name = username };
        await context.Users.AddAsync(user, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return user;
    }
}
