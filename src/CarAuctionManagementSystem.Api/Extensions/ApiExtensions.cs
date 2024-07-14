using Asp.Versioning;
using CarAuctionManagementSystem.Api.Endpoints.V1;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class ApiExtensions
{
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    public static void AddApiCors(this IServiceCollection services, string allowedHosts)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins(allowedHosts);
            });
        });
    }

    public static void MapApiEndpoints(this WebApplication application)
    {
        application.MapVersionEndpoint();

        var versionSet = application
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        var versionGroup = application
            .MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(versionSet);

        versionGroup.MapvVhicleEndpoints();
    }
}
