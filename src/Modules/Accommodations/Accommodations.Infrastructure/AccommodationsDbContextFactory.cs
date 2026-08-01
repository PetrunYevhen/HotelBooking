using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Accommodations.Infrastructure;

public class AccommodationsDbContextFactory : IDesignTimeDbContextFactory<AccommodationsDbContext>
{
    private readonly ILoggerFactory _loggerFactory;

    public AccommodationsDbContextFactory()
    {
    }

    public AccommodationsDbContextFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public AccommodationsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AccommodationsDbContext>();
        
        
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var user = configuration["POSTGRES_USER"] ?? "postgres";
            var password = configuration["POSTGRES_PASSWORD"] ?? "password";
            var database = configuration["POSTGRES_DB"] ?? "hotel_booking";
            var port = configuration["DB_PORT"] ?? "5433";
            connectionString = $"Host=localhost;Port={port};Database={database};Username={user};Password={password}";
        }

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(
                $"Connection string 'DefaultConnection' not found. Checked path: {basePath}"); 
        
        optionsBuilder
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(connectionString);
        
        return new AccommodationsDbContext(optionsBuilder.Options, _loggerFactory);
        
    }
}
