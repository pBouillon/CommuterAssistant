using MediatR;

namespace Assistant.Bot.Core.Messages;

public class GetNextDepartureRequest : BotRequest<string>
{
}

public class GetNextDepartureRequestHandler : IRequestHandler<GetNextDepartureRequest, string>
{
    public Task<string> Handle(GetNextDepartureRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.GetType().Name);
}
