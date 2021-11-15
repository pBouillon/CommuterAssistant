using MediatR;

namespace Assistant.Bot.Core.Messages;

public class SetWorkplaceLocationRequest : IRequest<string>
{
}

public class SetWorkplaceLocationRequestHandler : IRequestHandler<SetWorkplaceLocationRequest, string>
{
    public Task<string> Handle(SetWorkplaceLocationRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.GetType().Name);
}
