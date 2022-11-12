using Autofac;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Hosting;

namespace GRVAS.Training.CeuEmailCreator.HostedServices;

internal class HangfireJobServerService : IHostedService
{
    private BackgroundJobServer _backgroundJobServer;

    public HangfireJobServerService(
        ILifetimeScope lifetimeScope)
    {
        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMemoryStorage(new MemoryStorageOptions
            {
                FetchNextJobTimeout = TimeSpan.FromDays(1),
                JobExpirationCheckInterval = TimeSpan.FromDays(1)
            })
            .UseAutofacActivator(lifetimeScope);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundJobServer = new BackgroundJobServer(new BackgroundJobServerOptions
        {
            WorkerCount = 1
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _backgroundJobServer.Dispose();

        return Task.CompletedTask;
    }
}
