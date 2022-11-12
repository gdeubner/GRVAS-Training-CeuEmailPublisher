using GRVAS.Training.CeuEmailCreator.Job;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Hosting;

namespace GRVAS.Training.CeuEmailCreator.HostedServices;

internal class CeuEmailService : IHostedService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IEmailProcessor _emailProcessor;

    public CeuEmailService(
        IRecurringJobManager recurringJobManager,
        IEmailProcessor emailProcessor)
    {
        _recurringJobManager = recurringJobManager;
        _emailProcessor = emailProcessor;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting CEU class notifier");

        var cron = $"{DateTime.UtcNow.AddMinutes(1).Minute} {DateTime.UtcNow.Hour} * * *";

        _recurringJobManager.AddOrUpdate(
            "CEU class notifir",
            () => _emailProcessor.ProcessAsync(),
             $"{DateTime.UtcNow.AddMinutes(1).Minute} * * * *",
              TimeZoneInfo.Utc);

        var jobs = JobStorage.Current.GetConnection().GetRecurringJobs();
        var nextRun = jobs.First().NextExecution;
        Console.WriteLine($"Current Time: {DateTime.UtcNow} Next Run: {nextRun}");


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
