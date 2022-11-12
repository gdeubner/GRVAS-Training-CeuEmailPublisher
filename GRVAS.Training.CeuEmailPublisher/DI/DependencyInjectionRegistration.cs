using Amazon.SimpleEmail;
using Autofac;
using GRVAS.Training.CeuEmailCreator.Email;
using GRVAS.Training.CeuEmailCreator.HostedServices;
using GRVAS.Training.CeuEmailCreator.Job;
using GRVAS.Training.CeuEmailCreator.WebScrapers;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GRVAS.Training.CeuEmailCreator.DI;

internal static class DependencyInjectionRegistration
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //place holder

    }

    public static void ConfigureContainer(this ContainerBuilder builder, IConfiguration configuration)
    {
        // Hangfire
        builder.RegisterType<HangfireJobServerService>().As<IHostedService>().SingleInstance();
        builder.RegisterType<RecurringJobManager>().As<IRecurringJobManager>().SingleInstance();
        builder.RegisterType<CeuEmailService>().As<IHostedService>().InstancePerDependency();

        // Job
        builder.RegisterType<EmailProcessor>().AsSelf().As<IEmailProcessor>();

        //Email
        builder.RegisterType<EmailContentGenerator>().As<IEmailContentGenerator>().SingleInstance()
            .WithParameter("mountainsideUrl", configuration["MountainsideUrl"])
            .WithParameter("rwjbhUrl", configuration["RwjbhUrl"])
            .WithParameter("bergenCountyUrl", configuration["BergenCountyUrl"]);
        builder.RegisterType<EmailGenerator>().As<IEmailGenerator>().SingleInstance()
            .WithParameter("sender", configuration["SendingEmail"])
            .WithParameter("recipient", configuration["RecievingEmail"]);
        builder.RegisterInstance(_createEmailClient(configuration)).As<IAmazonSimpleEmailService>().SingleInstance();
        builder.RegisterType<EmailSender>().As<IEmailSender>().SingleInstance();

        // Web Scrapers
        builder.RegisterType<MountainsideWebScraper>().As<IMountainsideWebScraper>().SingleInstance();
        builder.RegisterType<RwjbhWebScraper>().As<IRwjbhWebScraper>().SingleInstance();

    }

    private static IAmazonSimpleEmailService _createEmailClient(IConfiguration configuration)
    {
        return new AmazonSimpleEmailServiceClient(
            configuration["AWS:Key"],
            configuration["AWS:Secret"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));
    }

}
