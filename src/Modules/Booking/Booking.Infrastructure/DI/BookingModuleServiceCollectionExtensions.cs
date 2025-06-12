using Booking.Domain.RepositoryContracts;
using Booking.Infrastructure.Dapper;
using Booking.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure.DI;

public static class BookingModuleServiceCollectionExtensions 
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register repositories
        services.AddScoped<IReservationWriteRepository, 
            Repositories.ReservationWriteRepository>();
        services.AddScoped<IReservationReadRepository, Repositories.ReservationReadRepository>();
        
        // Register application commands and queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Application.Command.AddReservationCommand>());

        
        // Register DbContext
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // Register Dapper type handlers
        SqlMapper.AddTypeHandler(new ReservationIdTypeHandler());
        return services;
    }
}