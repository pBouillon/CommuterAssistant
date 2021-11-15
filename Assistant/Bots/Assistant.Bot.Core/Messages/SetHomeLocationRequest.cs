﻿using MediatR;

namespace Assistant.Bot.Core.Messages;

public class SetHomeLocationRequest : BotRequest<string>
{
}

public class SetHomeLocationRequestHandler : IRequestHandler<SetHomeLocationRequest, string>
{
    public Task<string> Handle(SetHomeLocationRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.GetType().Name);
}
