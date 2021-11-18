using Assistant.Bot.Core.Commons.Messages.Handlers;
using Assistant.Bot.Core.Commons.Messages.Requests;
using Assistant.Contracts.Enums;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Messages;

/// <summary>
/// User request for the assistant to create or update his <see cref="CoordinateType.Work"/> coordinate based on the
/// provided <see cref="Contracts.ValueObjects.Location.GeoCoordinate"/>
/// </summary>
public class SetWorkplaceLocationRequest : BotLocationRequest<string> { }

/// <summary>
/// Request handler that will create or update the <see cref="CoordinateType.Work"/> location of the user, based on the
/// provided <see cref="Contracts.ValueObjects.Location.GeoCoordinate"/>
/// </summary>
public class SetWorkplaceLocationRequestHandler : LocationUpdateRequestHandler<SetWorkplaceLocationRequest, string>
{
    /// <inheritdoc />
    protected override string LocationUpdatedResponse
        => "The coordinate of your workplace has been successfully updated";

    /// <inheritdoc />
    protected override CoordinateType UpdatedCoordinateType
        => CoordinateType.Work;

    /// <inheritdoc />
    public SetWorkplaceLocationRequestHandler(
            ILogger<SetWorkplaceLocationRequestHandler> logger, IServiceScopeFactory serviceScopeFactory)
        : base(logger, serviceScopeFactory) { }
}
