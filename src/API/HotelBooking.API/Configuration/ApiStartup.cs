using Autofac;
using Facilities.Infrastructure.Configurations;
using HotelBooking.API.Modules.BookingManagement;
using HotelBooking.API.Modules.Facilities;
using HotelBooking.API.Modules.HotelManagement;
using HotelBooking.API.Modules.RoomManagement;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace HotelBooking.API.Configuration;

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
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new BookingManagementAutofacModule());
        containerBuilder.RegisterModule(new HotelManagementAutofacModule());
        containerBuilder.RegisterModule(new RoomManagementAutofacModule());
        containerBuilder.RegisterModule(new FacilitiesAutofacModule());
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
    public void InitializeModules(ILifetimeScope container)
    {
        BookingManagementStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        
        HotelStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        
        RoomManagementStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        FacilitiesStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
    }
}