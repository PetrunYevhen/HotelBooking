using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotelManagement.Infastructure;

public class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
{
    private readonly ILoggerFactory _loggerFactory;

    public HotelDbContextFactory()
    {
    }

    public HotelDbContextFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public HotelDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        
        
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException(
                $"Connection string 'DefaultConnection' not found. Checked path: {basePath}"); 
        
        optionsBuilder
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(
            connectionString
        );

        optionsBuilder.UseNpgsql(connectionString);
        
        return new HotelDbContext(optionsBuilder.Options, _loggerFactory);
        
    }
}