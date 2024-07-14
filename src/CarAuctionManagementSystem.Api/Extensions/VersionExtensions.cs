using System.Diagnostics;
using System.Reflection;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class VersionExtensions
{
    public static void MapVersionEndpoint(this WebApplication application, string pattern = "/version")
    {
        application
            .MapGet(pattern, () => GetInformationalVersion())
            .ShortCircuit();
    }

    public static string GetInformationalVersion(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetEntryAssembly();

        if (assembly is null)
        {
            return string.Empty;
        }

        return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion ?? string.Empty;
    }
}
