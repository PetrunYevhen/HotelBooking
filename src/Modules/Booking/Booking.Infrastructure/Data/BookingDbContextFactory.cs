using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Booking.Infrastructure.Data;

public class BookingDbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
{
    public BookingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
        
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\38095\\Programming\\ASP.NET_Projects\\HotelBooking\\HotelBooking\\src\\API\\HotelBooking.API"));
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
        
        return new BookingDbContext(optionsBuilder.Options);
    }
}