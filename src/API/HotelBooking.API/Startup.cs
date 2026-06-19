using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Accommodations.Infrastructure.Configuration;
using Bookings.Infrastructure.Configurations;
using HotelBooking.API.Modules.Accommodations;
using Payments.Infrastructure.Configuration;
using HotelBooking.API.Modules.Bookings;
using HotelBooking.API.Modules.Payments;
using Infrastructure.Client;
using Serilog;
using ILogger = Serilog.ILogger;

namespace HotelBooking.API;

public class Startup
{
    private const string ConnectionString = "DefaultConnection";
    private static ILogger _logger;
    private static ILogger _loggerForApi;
    private readonly IConfiguration _configuration;

    public Startup()
    {
        ConfigureLogger();
        
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        _loggerForApi.Information("Connection string:" + _configuration.GetConnectionString(ConnectionString));
        
        _loggerForApi.Information("Logger configured");
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new BookingsAutofacModule());
        containerBuilder.RegisterModule(new AccommodationsAutofacModule());
        containerBuilder.RegisterModule(new PaymentManagementAutofacModule());
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        ConfigureClient(services);
    }

    public void ConfigureClient(IServiceCollection services)
    {
        services.AddSingleton<InMemoryModuleClient>();
        services.AddSingleton<IClient>(sp => sp.GetRequiredService<InMemoryModuleClient>());
    }

    private static void ConfigureLogger()
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        _loggerForApi = _logger.ForContext("Module", "API");

        _loggerForApi.Information("Logger configured");
    }
    private string GetConnectionString()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");

        return connectionString;
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        var container = app.ApplicationServices.GetAutofacRoot();
        
        InitializeModules(container);
        
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelBooking API v1"));
        }

        app.UseRouting();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
    
    private void InitializeModules(ILifetimeScope container)
    { 
        var client = container.Resolve<InMemoryModuleClient>();
        
        BookingsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null,
            client);
        
        AccommodationsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null,
            client);
       
        
        PaymentsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
    }
}