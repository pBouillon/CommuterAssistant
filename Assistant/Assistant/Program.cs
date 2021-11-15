using Assistant;
using Assistant.Bot.Core;
using Assistant.Infrastructure;
using Assistant.Infrastructure.Persistence;
using Assistant.TelegramBot;

using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
    .CreateLogger();

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
