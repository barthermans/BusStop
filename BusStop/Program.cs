using BusStop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder()
    .ConfigureLogging(logging => { logging.ClearProviders(); })
    .ConfigureServices((context, services) =>
    {
        services
            .AddSingleton(args)
            .AddHostedService<Application>()
            .AddSingleton<IFileService, FileService>();
    }).StartAsync();