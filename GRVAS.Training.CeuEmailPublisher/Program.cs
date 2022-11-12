using Autofac;
using Autofac.Extensions.DependencyInjection;
using GRVAS.Training.CeuEmailCreator.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var environmentName = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

Console.WriteLine($"Environment: {environmentName}");
Console.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");

try
{
    var host = CreateHostBuilder(args, environmentName).Build();
    host.Run();
}
catch (Exception exc)
{
    Console.WriteLine($"Error running application due to {exc}");
}


static IHostBuilder CreateHostBuilder(string[] args, string env) =>
Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", false, false)
            .AddJsonFile($"appsettings.{env}.json", true, false);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        DependencyInjectionRegistration.ConfigureServices(services, hostContext.Configuration);
    })
    .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
    {
        DependencyInjectionRegistration.ConfigureContainer(builder, hostContext.Configuration);
    });