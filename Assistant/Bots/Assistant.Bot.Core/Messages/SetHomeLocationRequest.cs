using Assistant.Bot.Core.Commons.Messages;
using Assistant.Contracts.Enums;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Messages;

public class SetHomeLocationRequest : LocationUpdateRequest<string> { }

public class SetHomeLocationRequestHandler : LocationUpdateRequestHandler<SetHomeLocationRequest, string>
{
    protected override string LocationUpdatedResponse
        => "The coordinate of your home has been successfully updated";

    protected override CoordinateType UpdatedCoordinateType
        => CoordinateType.Home;

    public SetHomeLocationRequestHandler(
            ILogger<SetHomeLocationRequestHandler> logger, IServiceScopeFactory serviceScopeFactory)
        : base(logger, serviceScopeFactory) { }
}
