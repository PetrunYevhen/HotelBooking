using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Accommodations.Infrastructure.Configuration;
using Bookings.Infrastructure.Configurations;
using HotelBooking.API.Modules.Accommodations;
using Payments.Infrastructure.Configuration;
using Payments.Infrastructure.StripeGateway;
using HotelBooking.API.Modules.Bookings;
using HotelBooking.API.Modules.Payments;
using HotelBooking.API.Modules.Reviews;
using Infrastructure.Client;
using Infrastructure.Emails;
using Notifications.Infrastructure.Configuration;
using Reviews.Infrastructure.Configuration;
using Serilog;
using ILogger = Serilog.ILogger;

namespace HotelBooking.API;

public class Startup
{
    private static ILogger _logger;
    private static ILogger _loggerForApi;
    private readonly IConfiguration _configuration;

    public Startup()
    {
        ConfigureLogger();
        
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        _loggerForApi.Information("Configuration loaded");
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterModule(new BookingsAutofacModule());
        containerBuilder.RegisterModule(new AccommodationsAutofacModule());
        containerBuilder.RegisterModule(new PaymentsAutofacModule());
        containerBuilder.RegisterModule(new ReviewsAutofacModule());
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddHealthChecks();
        ConfigureClient(services);
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? ["http://localhost:5173"];

                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
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

    private SmtpSettings GetSmtpSettings()
    {
        return new SmtpSettings(
            _configuration["Smtp:Host"] ?? throw new InvalidOperationException("Smtp:Host not found in appsettings.json"),
            int.Parse(_configuration["Smtp:Port"] ?? throw new InvalidOperationException("Smtp:Port not found in appsettings.json")),
            _configuration["Smtp:From"] ?? throw new InvalidOperationException("Smtp:From not found in appsettings.json"),
            _configuration["Smtp:User"],
            _configuration["Smtp:Password"]);
    }

    private PaymentGatewaySettings GetPaymentGatewaySettings()
    {
        return new PaymentGatewaySettings(
            _configuration["Stripe:ApiBase"] ?? throw new InvalidOperationException("Stripe:ApiBase not found in appsettings.json"),
            _configuration["Stripe:ApiKey"] ?? throw new InvalidOperationException("Stripe:ApiKey not found in appsettings.json"),
            bool.Parse(_configuration["Stripe:IsMock"] ?? "false"));
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
        else
        {
            app.UseExceptionHandler();
        }

        app.UseRouting();
        app.UseCors("AllowReactApp");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
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
            null,
            GetPaymentGatewaySettings());
        
        ReviewsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null);
        
        NotificationsStartup.Initialize(
            GetConnectionString(),
            _logger,
            null,
            GetSmtpSettings());
    }
}
