using System.Diagnostics.CodeAnalysis;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class HeartbeatExtensions
{
    [ExcludeFromCodeCoverage]
    public static void MapHeartbeat(this WebApplication application, string pattern = "/heartbeat")
    {
        application
            .MapGet(pattern, () => "beating")
            .ShortCircuit();
    }
}
