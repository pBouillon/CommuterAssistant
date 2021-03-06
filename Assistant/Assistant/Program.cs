using Assistant;
using Assistant.Bot.Core;
using Assistant.Contracts.Entities;
using Assistant.Infrastructure;
using Assistant.Infrastructure.Persistence;
using Assistant.Bot.Telegram;

using Destructurama;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

// Logger code first's configuration
Log.Logger = new LoggerConfiguration()
    .Destructure.UsingAttributes()
    .Destructure.ByTransforming<User>(user => new
    {
        user.Id,
        user.Name,
    })
    .Destructure.ByTransforming<Coordinate>(coordinate => new
    {
        coordinate.Id,
        coordinate.Type,
        coordinate.Latitude,
        coordinate.Longitude,
    })
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .CreateLogger();

// Configure the host's dependency injection
IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddHostedService<Worker>()
            .AddInfrastructure(configuration)
            .AddBotCore()
            .AddTelegramBot();
    });

// Run the assistant
try
{
    Log.Information("Starting the assistant ..."); 
    var host = hostBuilder.Build(); 
    
    Log.Information("Initializing the database ...");
    await host.Services
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope()
        .ServiceProvider
        .GetRequiredService<ApplicationContext>()
        .Database
        .EnsureCreatedAsync();

    Log.Information("Starting the assistant ...");
    await host.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "The assistant terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
