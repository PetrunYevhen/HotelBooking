using System.Text;
using System.Security.Cryptography;
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
using Users.Infrastructure.Configuration;
using HotelBooking.API.Authentication;
using HotelBooking.API.Modules.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users.Application.Services;
using Users.Application.Command.Users.CreateAdmin;
using Users.Application.Contracts;
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
        containerBuilder.RegisterModule(new UsersAutofacModule());
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Paste an access token obtained from /api/auth/login."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = []
            });
        });
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddHealthChecks();
        ConfigureClient(services);
        ConfigureAuthentication(services);
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                var allowedOrigins = _configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? ["http://localhost:5173"];

                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public void ConfigureClient(IServiceCollection services)
    {
        services.AddSingleton<InMemoryModuleClient>();
        services.AddSingleton<IClient>(sp => sp.GetRequiredService<InMemoryModuleClient>());
    }

    private void ConfigureAuthentication(IServiceCollection services)
    {
        var settings = _configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        settings.Validate();
        services.AddSingleton(settings);
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = "unique_name",
                    RoleClaimType = "role"
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("AuthenticatedUser", policy => policy.RequireAuthenticatedUser());
        });
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
        app.UseAuthentication();
        app.UseAuthorization();

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

        UsersStartup.Initialize(
            GetConnectionString(),
            _logger,
            null
        );

        TryBootstrapFirstAdmin(container);
    }

    private void TryBootstrapFirstAdmin(ILifetimeScope container)
    {
        var suppliedSecret = Environment.GetEnvironmentVariable("HOTELBOOKING_BOOTSTRAP_ADMIN_SECRET");
        var expectedSecret = _configuration["BootstrapAdmin:Secret"];
        if (string.IsNullOrWhiteSpace(suppliedSecret) || string.IsNullOrWhiteSpace(expectedSecret) ||
            !CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(suppliedSecret), Encoding.UTF8.GetBytes(expectedSecret)))
            return;

        var username = _configuration["BootstrapAdmin:Username"];
        var password = _configuration["BootstrapAdmin:Password"];
        var email = _configuration["BootstrapAdmin:Email"];
        var firstName = _configuration["BootstrapAdmin:FirstName"];
        var lastName = _configuration["BootstrapAdmin:LastName"];
        var phoneNumber = _configuration["BootstrapAdmin:PhoneNumber"];
        if (new[] { username, password, email, firstName, lastName, phoneNumber }.Any(string.IsNullOrWhiteSpace))
        {
            _loggerForApi.Warning("Bootstrap admin was requested but its configuration is incomplete");
            return;
        }

        var users = container.Resolve<IUsersModule>();
        var result = users.ExecuteCommandAsync(new CreateAdminCommand
        {
            Username = username!, Password = password!, Email = email!, FirstName = firstName!, LastName = lastName!, PhoneNumber = phoneNumber!
        }).GetAwaiter().GetResult();
        if (result.IsSuccess)
            _loggerForApi.Information("Bootstrap administrator created");
    }
}
