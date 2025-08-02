using Autofac.Extensions.DependencyInjection;
using Autofac;
using Inventory.Worker;
using Serilog;
using Inventory.Application.Features.Products.Commands;
using Inventory.Application.Features.Customers.Queries;
using Inventory.Domain;
using Autofac.Core;
using DotNetEnv;
using Amazon.S3;
using Amazon.SQS;
using Amazon.Runtime;
Env.Load("web.env");

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
var migrationAssemblyName = typeof(Worker).Assembly;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Worker Started.............");
    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new WorkerModule(connectionString, migrationAssemblyName.FullName));

        })
    .ConfigureServices((context, services) =>
    {
        var awsOptions = configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonSQS>();
        services.AddAWSService<IAmazonS3>();

        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
       
        services.AddHostedService<Worker>();
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(migrationAssemblyName);
                cfg.RegisterServicesFromAssembly(typeof(GetCustomerByIdQuery).Assembly);
            }
            );
            
        })
        .Build();


    await host.RunAsync();

}
catch (Exception ex)
{
    Log.Fatal(ex,"Appstarted Failed");
}finally
{
    Log.CloseAndFlush();
}
