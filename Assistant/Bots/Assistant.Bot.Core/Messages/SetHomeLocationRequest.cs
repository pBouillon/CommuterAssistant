using Assistant.Bot.Core.Commons.Messages.Handlers;
using Assistant.Bot.Core.Commons.Messages.Requests;
using Assistant.Contracts.Enums;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Messages;

/// <summary>
/// User request for the assistant to create or update his <see cref="CoordinateType.Home"/> coordinate based on the
/// provided <see cref="Contracts.ValueObjects.Location.GeoCoordinate"/>
/// </summary>
public class SetHomeLocationRequest : BotLocationRequest<string> { }

/// <summary>
/// Request handler that will create or update the <see cref="CoordinateType.Home"/> location of the user, based on the
/// provided <see cref="Contracts.ValueObjects.Location.GeoCoordinate"/>
/// </summary>
public class SetHomeLocationRequestHandler : LocationUpdateRequestHandler<SetHomeLocationRequest, string>
{
    /// <inheritdoc />
    protected override string LocationUpdatedResponse
        => "The coordinate of your home has been successfully updated";

    /// <inheritdoc />
    protected override CoordinateType UpdatedCoordinateType
        => CoordinateType.Home;

    /// <inheritdoc />
    public SetHomeLocationRequestHandler(
            ILogger<SetHomeLocationRequestHandler> logger, IServiceScopeFactory serviceScopeFactory)
        : base(logger, serviceScopeFactory) { }
}
