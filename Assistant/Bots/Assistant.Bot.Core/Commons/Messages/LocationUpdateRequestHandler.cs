using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;
using Assistant.Contracts.Enums;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Commons.Messages;

public abstract class LocationUpdateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : BotLocationRequest<TResponse>
{
    private readonly ILogger<LocationUpdateRequestHandler<TRequest, TResponse>> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected abstract TResponse LocationUpdatedResponse { get; }

    protected abstract CoordinateType UpdatedCoordinateType { get; }

    protected LocationUpdateRequestHandler(
            ILogger<LocationUpdateRequestHandler<TRequest, TResponse>> logger, IServiceScopeFactory serviceScopeFactory)
        => (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);

    private Coordinate CreateCoordinate(User requester, TRequest request)
    {
        _logger.LogInformation("No known work coordinate for {@User}, creating it", requester.Name);

        var homeCoordinate = new Coordinate
        {
            Longitude = request.Location.Longitude,
            Latitude = request.Location.Latitude,
            Type = UpdatedCoordinateType,
        };

        requester.Coordinates.Add(homeCoordinate);

        return homeCoordinate;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        // See: https://stackoverflow.com/a/48368934/6152689
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationContext>();

        var requester = await context.Users
            .Include(user => user.Coordinates)
            .SingleAsync(user => user.Name == request.Context.SenderUsername, cancellationToken);

        var homeCoordinate = requester.Coordinates
            .SingleOrDefault(coordinate => coordinate.Type == UpdatedCoordinateType);

        homeCoordinate = homeCoordinate is null
                ? CreateCoordinate(requester, request)
                : UpdateCoordinate(homeCoordinate, request);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
                    "Work coordinate of {@User} successfully updated to {@Coordinate}",
                    requester, homeCoordinate);

        return LocationUpdatedResponse;
    }

    private Coordinate UpdateCoordinate(Coordinate coordinate, TRequest request)
    {
        coordinate.Longitude = request.Location.Longitude;
        coordinate.Latitude = request.Location.Latitude;

        return coordinate;
    }
}

