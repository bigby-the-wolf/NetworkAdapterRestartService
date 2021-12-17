using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using NetworkAdapterRestartService;
using NetworkAdapterRestartService.Interfaces;           

var hostBuilder =
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(configureLogging =>
            configureLogging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
        .ConfigureServices((_, services) =>
        {
            services
                .AddTransient<IConnectionVerifier, EthernetConnectionVerifier>()
                .AddTransient<INetworkAdapterProvider, WindowsNetworkAdapterProvider>()
                .AddTransient<IConnectionValidator, SimpleConnectionValidator>();

            services
                .AddHostedService<NetworkAdapterResetWorker>()
                .Configure<EventLogSettings>(config =>
                {
                    config.LogName = "Network Adapter Restart Service";
                    config.SourceName = "My Hosted Services Source";
                })
                .Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(5));
        }).UseWindowsService();

hostBuilder.Build().Run();