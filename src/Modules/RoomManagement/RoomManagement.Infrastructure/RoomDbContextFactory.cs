using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RoomManagement.Infrastructure;

public class RoomDbContextFactory : IDesignTimeDbContextFactory<RoomDbContext>
{
    private readonly ILoggerFactory _loggerFactory;
    
    public RoomDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RoomDbContext>();
        
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder
            .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(connectionString);
        
        return new RoomDbContext(optionsBuilder.Options, _loggerFactory);
    }
}