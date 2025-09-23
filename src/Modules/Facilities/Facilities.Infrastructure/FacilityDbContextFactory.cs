using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Facilities.Infrastructure;

public class FacilityDbContextFactory : IDesignTimeDbContextFactory<FacilitiesDbContext>
{
    private readonly ILoggerFactory _loggerFactory;
    public FacilitiesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FacilitiesDbContext>();
        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),  @"..\..\..\API\HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(connectionString);
        
        return new FacilitiesDbContext(optionsBuilder.Options, _loggerFactory);
    }
}