using Assistant.Bot.Core.Commons.Messages;
using Assistant.Contracts.Enums;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Messages;

public class SetWorkplaceLocationRequest : BotLocationRequest<string> { }

public class SetWorkplaceLocationRequestHandler : LocationUpdateRequestHandler<SetWorkplaceLocationRequest, string>
{
    protected override string LocationUpdatedResponse
        => "The coordinate of your workplace has been successfully updated";

    protected override CoordinateType UpdatedCoordinateType
        => CoordinateType.Work;

    public SetWorkplaceLocationRequestHandler(
            ILogger<SetWorkplaceLocationRequestHandler> logger, IServiceScopeFactory serviceScopeFactory)
        : base(logger, serviceScopeFactory) { }
}
