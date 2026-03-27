using Infrastructure.TypedIdConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PaymentManagement.Infrastructure;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    private readonly ILoggerFactory _loggerFactory;

    public PaymentDbContextFactory()
    {
    }

    public PaymentDbContextFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public PaymentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
        
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),  @"../HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
            .UseNpgsql(connectionString);
        
        return new PaymentDbContext(optionsBuilder.Options, _loggerFactory);
        
    }
}