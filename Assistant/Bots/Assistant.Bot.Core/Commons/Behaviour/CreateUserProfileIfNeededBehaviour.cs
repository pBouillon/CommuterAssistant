using Assistant.Bot.Core.Messages;
using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Behaviour;

public class CreateUserProfileIfNeededBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : BotRequest<TResponse>, IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    private readonly IServiceProvider _services;

    public CreateUserProfileIfNeededBehaviour(ILogger<TRequest> logger, IServiceProvider services)
        => (_logger, _services) = (logger, services);

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        using (var scope = _services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<IApplicationContext>();

            var requester = context.Users.SingleOrDefault(user => user.Name == user.Name);

            if (requester is null)
            {
                var user = new User { Name = request.Context.SenderUsername };
                context.Users.Add(user);

                context.SaveChanges();
                _logger.LogInformation("User {User} has been created", new { user.Id, user.Name });
            }
        }

        return await next();
    }
}
