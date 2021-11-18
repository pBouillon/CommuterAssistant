using Assistant.Bot.Core.Commons.Messages.Requests;

using MediatR;

namespace Assistant.Bot.Core.Messages;

/// <summary>
/// User request for the assistant to retrieve the next departure to get the user to the location where he is not. If
/// he is closer to its <see cref="Contracts.Enums.CoordinateType.Home"/>, get the next departure for his
/// <see cref="Contracts.Enums.CoordinateType.Work"/> and vice versa.
/// </summary>
public class GetNextDepartureRequest : BotLocationRequest<string> { }

/// <summary>
/// Request handler that will search for the next departure for the requester to get him where he is not
/// </summary>
public class GetNextDepartureRequestHandler : IRequestHandler<GetNextDepartureRequest, string>
{
    /// <inheritdoc />
    public Task<string> Handle(GetNextDepartureRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new { request.Location.Latitude, request.Location.Longitude }.ToString()!);
}
