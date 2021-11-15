using MediatR;

namespace Assistant.Bot.Core.Messages;

public class SetWorkplaceLocationRequest : BotRequest<string>
{
}

public class SetWorkplaceLocationRequestHandler : IRequestHandler<SetWorkplaceLocationRequest, string>
{
    public Task<string> Handle(SetWorkplaceLocationRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.GetType().Name);
}
