using Assistant.Bot.Core.Services;
using Assistant.Contracts.Entities;
using Assistant.Contracts.Enums;
using Assistant.Contracts.ValueObjects.Location;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Assistant.Bot.Core.Messages;

public class SetHomeLocationRequest : BotRequest<string>
{
    public GeoCoordinate Coordinate { get; init; } = new();
}

public class SetHomeLocationRequestHandler : IRequestHandler<SetHomeLocationRequest, string>
{
    private readonly ILogger<SetHomeLocationRequestHandler> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SetHomeLocationRequestHandler(ILogger<SetHomeLocationRequestHandler> logger, IServiceScopeFactory serviceScopeFactory)
        => (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);

    public async Task<string> Handle(SetHomeLocationRequest request, CancellationToken cancellationToken)
    {
        // See: https://stackoverflow.com/a/48368934/6152689
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationContext>();

        var requester = await context.Users
            .Include(user => user.Coordinates)
            .SingleAsync(user => user.Name == request.Context.SenderUsername, cancellationToken);

        var homeCoordinate = requester.Coordinates
            .SingleOrDefault(coordinate => coordinate.Type == CoordinateType.Home);

        homeCoordinate = homeCoordinate is null
            ? CreateCoordinate(requester, request)
            : UpdateCoordinate(homeCoordinate, request);

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Home coordinate of {@User} successfully updated to {@Coordinate}",
            requester, homeCoordinate);

        return "The coordinate of your home has been successfully updated";
    }

    private Coordinate CreateCoordinate(User requester, SetHomeLocationRequest request)
    {
        _logger.LogInformation("No known home coordinate for {@User}, creating it", requester.Name);

        var homeCoordinate = new Coordinate
        {
            Longitude = request.Coordinate.Longitude,
            Latitude = request.Coordinate.Latitude,
            Type = CoordinateType.Home,
        };

        requester.Coordinates.Add(homeCoordinate);

        return homeCoordinate;
    }

    private Coordinate UpdateCoordinate(Coordinate coordinate, SetHomeLocationRequest request)
    {
        coordinate.Longitude = request.Coordinate.Longitude;
        coordinate.Latitude = request.Coordinate.Latitude;

        return coordinate;
    }
}
