using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bookings.Infrastructure.Configurations;
using Facilities.Infrastructure.Configurations;
using HotelBooking.API.Modules.Bookings;
using HotelBooking.API.Modules.Hotels;
using HotelBooking.API.Modules.Payments;
using HotelBooking.API.Modules.Rooms;
using Hotels.Infastructure.Configuration;
using Infrastructure.Client;
using Payments.Infrastructure.Configuration;
using Rooms.Infrastructure.Configuration;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace HotelBooking.API;

public class ApiStartup
{
    private const string ConnectionString = "DefaultConnection";
    private static ILogger _logger;
    private static ILogger _loggerForApi;
    private readonly IConfiguration _configuration;

    public ApiStartup()
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
        containerBuilder.RegisterModule(new HotelsAutofacModule());
        containerBuilder.RegisterModule(new RoomManagementAutofacModule());
        // containerBuilder.RegisterModule(new FacilitiesAutofacModule());
        containerBuilder.RegisterModule(new PaymentManagementAutofacModule());
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
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
            .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
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
        
        HotelStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        
        RoomsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null,
            client);
        FacilitiesStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        
        PaymentsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
    }
}