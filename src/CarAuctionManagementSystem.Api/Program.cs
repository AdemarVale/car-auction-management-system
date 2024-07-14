using CarAuctionManagementSystem.Api.Extensions;
using CarAuctionManagementSystem.Api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAppHealthChecks();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();

builder.Services.AddCustomApiVersioning();
builder.Services.AddSwagger();
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

builder.Services.AddApiCors(builder.Configuration["AllowedHosts"]!);

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

try
{
    app.MapApiEndpoints();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    app.ApplyMigrations(app.Configuration);

    app.UseRouting();
    app.UseCors();

    app.UseHttpsRedirection();
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    app.MapHealthChecks("/healthz").ShortCircuit();
    app.MapHeartbeat("/heartbeat");

    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Unhandled exception");
    throw;
}
finally
{
    app.Logger.LogInformation("Shut down complete");
}

public partial class Program
{
}
