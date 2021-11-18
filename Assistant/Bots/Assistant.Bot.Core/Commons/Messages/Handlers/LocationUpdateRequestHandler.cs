using Assistant.Bot.Core.Commons.Messages.Requests;
using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;
using Assistant.Contracts.Enums;
using Assistant.Contracts.ValueObjects.Location;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Messages.Handlers;

/// <summary>
/// Request handler that will update the location of a <see cref="User"/>, based on the desired
/// <see cref="CoordinateType"/>
/// </summary>
/// <typeparam name="TRequest">The incoming <see cref="BotLocationRequest{TResponse}"/></typeparam>
/// <typeparam name="TResponse">The type of the response to be returned</typeparam>
public abstract class LocationUpdateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : BotLocationRequest<TResponse>
{
    /// <summary>
    /// The request's logger
    /// </summary>
    private readonly ILogger<LocationUpdateRequestHandler<TRequest, TResponse>> _logger;

    /// <summary>
    /// The service provider used to create a scope and access a scoped instance of the
    /// <see cref="IApplicationContext"/>
    /// </summary>
    /// <remarks>
    /// To see why the <see cref="IApplicationContext"/> is not directly injected,
    /// see <a href="https://stackoverflow.com/a/48368934/6152689">this StackOverflow answer</a>
    /// </remarks>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// The response to be returned when the location has been successfully updated
    /// </summary>
    protected abstract TResponse LocationUpdatedResponse { get; }

    /// <summary>
    /// The type of the coordinate to update
    /// </summary>
    protected abstract CoordinateType UpdatedCoordinateType { get; }

    /// <summary>
    /// Create a new instance of the <see cref="LocationUpdateRequestHandler"/>
    /// </summary>
    /// <param name="logger">The request's logger</param>
    /// <param name="serviceScopeFactory">
    /// The service provider used to create a scope and access a scoped instance of the
    /// <see cref="IApplicationContext"/>
    /// </param>
    protected LocationUpdateRequestHandler(
            ILogger<LocationUpdateRequestHandler<TRequest, TResponse>> logger,
            IServiceScopeFactory serviceScopeFactory)
        => (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);

    /// <summary>
    /// Create a new coordinate of type <see cref="CoordinateType"/> bound to the requested
    /// </summary>
    /// <param name="requester">The <see cref="User"/> emitting the request</param>
    /// <param name="location">The location to create</param>
    /// <returns>The created coordinate</returns>
    private Coordinate CreateUserCoordinate(User requester, GeoCoordinate location)
    {
        _logger.LogInformation("No known work coordinate for {@User}, creating it", requester.Name);

        var homeCoordinate = new Coordinate
        {
            Longitude = location.Longitude,
            Latitude = location.Latitude,
            Type = UpdatedCoordinateType,
        };

        requester.Coordinates.Add(homeCoordinate);

        return homeCoordinate;
    }

    /// <summary>
    /// Update the user's location based on the location provided in the request and the
    /// <see cref="UpdatedCoordinateType"/>
    /// </summary>
    /// <param name="request">The request's payload</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>
    /// An awaitable task returning <see cref="LocationUpdatedResponse"/> when the coordinate has been updated
    /// </returns>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationContext>();

        var requester = await context.Users
            .Include(user => user.Coordinates)
            .SingleAsync(user => user.Name == request.Context.SenderUsername, cancellationToken);

        var homeCoordinate = requester.Coordinates
            .SingleOrDefault(coordinate => coordinate.Type == UpdatedCoordinateType);

        homeCoordinate = homeCoordinate is null
                ? CreateUserCoordinate(requester, request.Location)
                : UpdateCoordinate(homeCoordinate, request.Location);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
                    "Work coordinate of {@User} successfully updated to {@Coordinate}",
                    requester, homeCoordinate);

        return LocationUpdatedResponse;
    }

    /// <summary>
    /// Update the <paramref name="coordinate"/> with the values of the provided <paramref name="location"/>
    /// </summary>
    /// <param name="coordinate">The coordinate to update</param>
    /// <param name="location">The location to create</param>
    /// <returns>The created coordinate</returns>
    private Coordinate UpdateCoordinate(Coordinate coordinate, GeoCoordinate location)
    {
        coordinate.Longitude = location.Longitude;
        coordinate.Latitude = location.Latitude;

        return coordinate;
    }
}

