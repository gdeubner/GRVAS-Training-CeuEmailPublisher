// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.

using GRVAS.Training.CeuEmailTrigger.Validation;
using Microsoft.Extensions.Configuration;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GRVAS.Training.CeuEmailTrigger;

internal class Function
{
    private const string AWS_USE_APP_CONFIG = "AWS:UseAppConfig";
    private const string AWS_KEY = "AWS:Key";
    private const string AWS_SECRET = "AWS:Secret";
    private const string AWS_REGION = "AWS:Region";

    private const string MOUNTAINSIDE_URL = "MOUNTAINSIDE_URL";
    private const string RWJ_URL = "RWJ_URL";
    private const string BERGEN_COUNTY_URL = "BERGEN_COUNTY_URL";

    private const string SENDER_EMAIL = "SENDER_EMAIL";
    private const string DESTINATION_EMAIL = "DESTINATION_EMAIL";
    private const string FAILURE_EMAIL = "FAILURE_EMAIL";


    public bool FunctionHandler()
    {
        try
        {
            LambdaLogger.Log("Ceu Email Function Triggered");

            // Start up container
            var serviceProvider = ConfigureServices();

            var messageProcessor = serviceProvider.GetService<IEmailProcessor>();

            messageProcessor?.ProcessAsync();

            LambdaLogger.Log("Function Finished");
            return true;
        }
        catch (Exception e)
        {
            LambdaLogger.Log($"Function finished with exception: [{e}]");
            return true;
        }
    }

    private static IServiceProvider ConfigureServices()
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.dev.json", optional: false)
               .Build();

            // Environment Variables
            var environment = (Environment.GetEnvironmentVariable("ENV") ?? "dev").ToLower();

            var builder = new ContainerBuilder();
            var services = new ServiceCollection();

            // Services
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddLambdaLogger();
            });

            var senderEmail = Environment.GetEnvironmentVariable(SENDER_EMAIL);
            var destinationEmail = Environment.GetEnvironmentVariable(DESTINATION_EMAIL);
            var failureEmail = Environment.GetEnvironmentVariable(FAILURE_EMAIL);


            // Job
            builder.RegisterType<EmailProcessor>().AsSelf().As<IEmailProcessor>()
                .WithParameter("mountainsideUrl", configuration[MOUNTAINSIDE_URL])
                .WithParameter("rwjbhUrl", configuration[RWJ_URL]);

            //Email
            builder.RegisterType<EmailContentGenerator>().As<IEmailContentGenerator>().SingleInstance()
                .WithParameter("mountainsideUrl", configuration[MOUNTAINSIDE_URL])
                .WithParameter("rwjbhUrl", configuration[RWJ_URL])
                .WithParameter("bergenCountyUrl", configuration[BERGEN_COUNTY_URL]);
            builder.RegisterType<EmailGenerator>().As<IEmailGenerator>().SingleInstance()
                .WithParameter("sender", senderEmail)
                .WithParameter("recipient", destinationEmail)
                .WithParameter("failureRecipient", failureEmail);

            builder.RegisterInstance(_createEmailClient()).As<IAmazonSimpleEmailService>().SingleInstance();
            builder.RegisterType<EmailSender>().As<IEmailSender>().SingleInstance();

            // Web Scrapers
            builder.RegisterType<MountainsideWebScraper>().As<IMountainsideWebScraper>().SingleInstance();
            builder.RegisterType<RwjbhWebScraper>().As<IRwjbhWebScraper>().SingleInstance();

            //validation
            builder.RegisterType<ClassValidator>().As<IClassValidator>().SingleInstance();


            //Build 
            builder.Populate(services);
            var container = builder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }
        catch (Exception e)
        {
            LambdaLogger.Log($"An error occured while configuring services for the lambda. Ex: {e}");

            return null;
        }
    }

    private static IAmazonSimpleEmailService _createEmailClient()
    {
        return Convert.ToBoolean(Environment.GetEnvironmentVariable(AWS_USE_APP_CONFIG)) ? 
            new AmazonSimpleEmailServiceClient(
                Environment.GetEnvironmentVariable(AWS_KEY),
                Environment.GetEnvironmentVariable(AWS_SECRET),
                RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable(AWS_REGION))) :
            new AmazonSimpleEmailServiceClient();
    }
}